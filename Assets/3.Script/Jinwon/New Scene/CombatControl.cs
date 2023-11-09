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
    private bool hasGun = false; // �κ��丮�� ������ ���� �ִ°�?
    private bool equipGun = false; // ���� �������ΰ�?

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
        // [�� ȹ��] - TEST
        if (!hasGun && Input.GetKeyDown(KeyCode.Return))
        {
            hasGun = true;

            currentGun = Instantiate(testGunPrefab, transform.position, Quaternion.identity);
            currentGun.transform.SetParent(backGunPivot);
            currentGun.transform.localPosition = Vector3.zero;
            currentGun.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        // [�� ����] - TEST
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

        // [������]
        if (equipGun && Input.GetKeyDown(KeyCode.R))
        {

        }

        // [���콺 �Է�]
        // 1. ��Ŭ�� -> Ÿ�̸� ����
        // 2. ��Ŭ�� ���� �� Ÿ�̸ӿ� ����
        // ��¦ �������� 1��Ī ����
        // ���� 3��Ī ���¿����� 3��Ī ����
        // ���� 1��Ī ���¿����� 1��Ī ����
        // 3. ��Ŭ�� �� ä�� ���� ������ 3��Ī ����

        // [��Ŭ��]
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
                    // 3��Ī ����
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
                    // 1��Ī ����
                    isFirstPerson = false;
                    cm.currentSpeed = cm.walkSpeed;
                    zoomControl.First_ZoomOut();

                    rig.transform.Find("Aim").GetComponent<MultiAimConstraint>().weight = 0f;
                    rig.transform.Find("Body").GetComponent<MultiAimConstraint>().weight = 0f;

                    return;
                }

                if (isThirdPerson)
                {
                    // 3��Ī ����
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
                    // 1��Ī ����
                    isFirstPerson = true;
                    cm.currentSpeed = cm.firstPersonSpeed;
                    zoomControl.First_ZoomIn(currentGun);
                    return;
                }
            }
        }

        // [��Ŭ��]
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

        // [�ѷ�����]
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

        // [Aim ����]
        if (isFirstPerson || isThirdPerson)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f)); // ȭ�� �߾� (ũ�ν���� ��ġ)�� Ray ���
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
            pov.m_HorizontalAxis.Value += Random.Range(-recoilX, recoilX); // x�� �ݵ�;
            pov.m_VerticalAxis.Value -= Random.Range(0, recoilY); // y�� �ݵ�
        }
        else if (isThirdPerson)
        {
            zoomControl.thirdPersonCamera.m_XAxis.Value += Random.Range(-recoilX * 0.01f, recoilX * 0.01f); // x�� �ݵ�;
            zoomControl.thirdPersonCamera.m_YAxis.Value -= Random.Range(0, recoilY * 0.01f); // y�� �ݵ�;
        }
    }
}
