using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Cinemachine;

public class CombatControl : MonoBehaviour
{
    // Gun
    [Header("Gun")]
    private GameObject currentGun;
    [SerializeField] private GameObject testGunPrefab;
    private bool hasGun = false; // 인벤토리에 장착할 총이 있는가?
    private bool equipGun = false; // 총을 장착중인가?

    [Header("Recoil")]
    public float recoilX;
    public float recoilY;

    // Components
    private Animator animator;
    private CharacterMovement cm;
    private ZoomControl zoomControl;

    // Mouse Input
    [Header("Mouse Input")]
    public bool timerOn = false;
    public float clickTimer = 0f;

    // Zoom
    [Header("Zoom Status")]
    public bool isFirstPerson = false;
    public bool isThirdPerson = false;
    private float thirdPersonEnterTime = 0.25f;

    // Aim
    [Header("Aim")]
    [SerializeField] private GameObject aimTarget;
    [SerializeField] private GameObject rig;
    [SerializeField] private Transform backGunPivot;
    public Transform holdGunPivot;
    [SerializeField] private Transform shootGunPivot;
    public bool lookAround = false;
    public float normalCamX;
    public float normalCamY;

    private void Awake()
    {
        TryGetComponent(out animator);
        TryGetComponent(out cm);
        TryGetComponent(out zoomControl);
    }

    private void Update()
    {
        // [총 획득] - TEST
        if (!hasGun && Input.GetKeyDown(KeyCode.Return))
        {
            hasGun = true;

            currentGun = Instantiate(testGunPrefab, transform.position, Quaternion.identity);
            currentGun.transform.SetParent(backGunPivot);
            currentGun.transform.localPosition = Vector3.zero;
            currentGun.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        // [총 장착] - TEST
        if (hasGun && !equipGun && Input.GetKeyDown(KeyCode.Keypad1))
        {
            equipGun = true;
            rig.GetComponent<Rig>().weight = 1.0f;
            animator.SetBool("EquipGun", true);
            animator.SetTrigger("Equip");

            currentGun.transform.SetParent(holdGunPivot);
            currentGun.transform.localPosition = Vector3.zero;
            currentGun.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        // [재장전]
        if (equipGun && Input.GetKeyDown(KeyCode.R))
        {

        }

        // [마우스 입력]
        // 1. 우클릭 -> 타이머 시작
        // 2. 우클릭 해제 시 타이머에 따라
        // 살짝 눌렀으면 1인칭 진입
        // 원래 3인칭 상태였으면 3인칭 해제
        // 원래 1인칭 상태였으면 1인칭 해제
        // 3. 우클릭 한 채로 오래 있으면 3인칭 진입

        // [우클릭]
        if (equipGun && cm.isGround)
        {
            if (Input.GetMouseButtonDown(1))
            {
                timerOn = true;
            }

            if (timerOn)
            {
                clickTimer += Time.deltaTime;
            }

            if (clickTimer >= thirdPersonEnterTime)
            {
                timerOn = false;
                clickTimer = 0;

                if (!isFirstPerson && !isThirdPerson)
                {
                    // 3인칭 진입
                    isThirdPerson = true;
                    cm.currentSpeed = cm.thirdPersonSpeed;
                    zoomControl.Third_ZoomIn();

                    rig.transform.Find("Aim").GetComponent<MultiAimConstraint>().weight = 1.0f;
                    rig.transform.Find("Body").GetComponent<MultiAimConstraint>().weight = 1.0f;

                    currentGun.transform.SetParent(shootGunPivot);
                    currentGun.transform.localPosition = Vector3.zero;
                    currentGun.transform.localRotation = Quaternion.Euler(Vector3.zero);
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                timerOn = false;
                clickTimer = 0;

                if (isFirstPerson)
                {
                    // 1인칭 해제
                    isFirstPerson = false;
                    cm.currentSpeed = cm.walkSpeed;
                    zoomControl.First_ZoomOut();

                    rig.transform.Find("Aim").GetComponent<MultiAimConstraint>().weight = 0f;
                    rig.transform.Find("Body").GetComponent<MultiAimConstraint>().weight = 0f;

                    return;
                }

                if (isThirdPerson)
                {
                    // 3인칭 해제
                    isThirdPerson = false;
                    cm.currentSpeed = cm.walkSpeed;
                    zoomControl.Third_ZoomOut();

                    rig.transform.Find("Aim").GetComponent<MultiAimConstraint>().weight = 0f;
                    rig.transform.Find("LeftHand").GetComponent<TwoBoneIKConstraint>().weight = 1.0f;

                    currentGun.transform.SetParent(holdGunPivot);
                    currentGun.transform.localPosition = Vector3.zero;
                    currentGun.transform.localRotation = Quaternion.Euler(Vector3.zero);

                    return;
                }

                if (!isFirstPerson && !isThirdPerson && clickTimer < thirdPersonEnterTime)
                {
                    // 1인칭 진입
                    isFirstPerson = true;
                    cm.currentSpeed = cm.firstPersonSpeed;
                    zoomControl.First_ZoomIn(currentGun);
                    return;
                }
            }
        }

        // [좌클릭]
        if (equipGun && Input.GetMouseButton(0))
        {
            if (isFirstPerson || isThirdPerson)
            {
                if (currentGun != null)
                {
                    currentGun.GetComponent<Gun>().PlayerShoot();
                    GunRecoil();
                }
            }
        }

        // [둘러보기]
        if (!isFirstPerson && !isThirdPerson && !lookAround && Input.GetKeyDown(KeyCode.LeftAlt))
        {
            zoomControl.StartLookAround();
            lookAround = true;
        }
        if (lookAround && Input.GetKeyUp(KeyCode.LeftAlt))
        {
            zoomControl.EndLookAround();
            lookAround = false;
        }

        // [Aim 설정]
        if (isFirstPerson || isThirdPerson)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f)); // 화면 중앙 (크로스헤어 위치)에 Ray 쏘기
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 100f))
            {
                aimTarget.transform.position = raycastHit.point;
            }
        }
        else
        {
            // body weight = 0
            rig.transform.Find("Body").GetComponent<MultiAimConstraint>().weight = 0.0f;
        }
    }

    public void EquipGun(GameObject gun)
    {
        currentGun = gun;
    }

    private void GunRecoil()
    {
        if (isFirstPerson)
        {
            CinemachinePOV pov = zoomControl.firstPersonCamera.GetCinemachineComponent<CinemachinePOV>();
            pov.m_HorizontalAxis.Value += Random.Range(-recoilX, recoilX); // x축 반동;
            pov.m_VerticalAxis.Value -= Random.Range(0, recoilY); // y축 반동
        }
        else if (isThirdPerson)
        {
            zoomControl.thirdPersonCamera.m_XAxis.Value += Random.Range(-recoilX * 0.01f, recoilX * 0.01f); // x축 반동;
            zoomControl.thirdPersonCamera.m_YAxis.Value -= Random.Range(0, recoilY * 0.01f); // y축 반동;
        }
    }
}
