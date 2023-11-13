using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Cinemachine;

public enum Weapon
{
    None,
    Gun,
    Grenade
}

public class CombatControl : MonoBehaviour
{
    // Camera
    [Header("Camera")]
    [SerializeField] private Transform mainCamera; // ����ī�޶�

    // Weapon Stat
    public Weapon currentWeapon = Weapon.None;

    // Gun
    [Header("Gun")]
    public GameObject currentGun;
    [SerializeField] private GameObject testGunPrefab;
    private bool hasGun = false; // �κ��丮�� ������ ���� �ִ°�?
    private bool isReloading = false; // ������ ���ΰ�?

    // Grenade
    [Header("Grenade")]
    public Transform grenadePivot; // ����ź �ǹ�
    private bool hasGrenade = false; // �κ��丮�� ����ź�� �ִ°�?
    [SerializeField] private GameObject testGrenadePrefab; // ����ź ������
    public float throwPower = 7.5f; // ������ ��
    public Vector3 throwDirection; // ���� ����
    private bool canThrow = true; // ������ ���� ����

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
        // [���� �� �ڿ� ����] - TEST
        if (!hasGun && Input.GetKeyDown(KeyCode.Return))
        {
            hasGun = true;

            currentGun = Instantiate(testGunPrefab, transform.position, Quaternion.identity);
            currentGun.transform.SetParent(backGunPivot);
            currentGun.transform.localPosition = Vector3.zero;
            currentGun.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        // [���� �տ� ����] - TEST
        if (hasGun && (currentWeapon != Weapon.Gun) && Input.GetKeyDown(KeyCode.Keypad1))
        {
            currentWeapon = Weapon.Gun;
            rig.GetComponent<Rig>().weight = 1.0f;
            animator.SetBool("EquipGun", true);
            animator.SetTrigger("Equip");

            currentGun.transform.SetParent(holdGunPivot);
            currentGun.transform.localPosition = Vector3.zero;
            currentGun.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        // [������]
        if (currentWeapon == Weapon.Gun && !isReloading && Input.GetKeyDown(KeyCode.R))
        {
            if (currentGun != null && currentGun.GetComponent<Gun>().currentMag != currentGun.GetComponent<Gun>().magSize && InventoryControl.instance.CheckInventory(108))
            {
                isReloading = true;
                StartCoroutine(Reload_co());
            }
        }

        // [����ź ȹ��] - TEST
        if (!hasGrenade && Input.GetKeyDown(KeyCode.T))
        {
            hasGrenade = true;
        }

        // [����ź ����]
        if (hasGrenade && Input.GetKeyDown(KeyCode.Y))
        {
            if (isFirstPerson || isThirdPerson)
            {
                return;
            }

            currentWeapon = Weapon.Grenade;

            StartCoroutine(UnEquipGun_co());
        }

        // [���콺 �Է�]
        // 1. ��Ŭ�� -> Ÿ�̸� ����
        // 2. ��Ŭ�� ���� �� Ÿ�̸ӿ� ����
        // ��¦ �������� 1��Ī ����
        // ���� 3��Ī ���¿����� 3��Ī ����
        // ���� 1��Ī ���¿����� 1��Ī ����
        // 3. ��Ŭ�� �� ä�� ���� ������ 3��Ī ����

        float camAngle;
        if (mainCamera.eulerAngles.x > 300)
        {
            camAngle = mainCamera.eulerAngles.x - 360.0f;
        }
        else
        {
            camAngle = mainCamera.eulerAngles.x;
        }

        float upPower = 1.0f - camAngle / 30.0f;
        throwDirection = transform.up * upPower + transform.forward; // ���콺 ȸ���� ���� ����ź ��ô ���� ����

        // [�� & ��Ŭ��]
        if (currentWeapon == Weapon.Gun && cm.isGround && !isReloading)
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

                    currentGun.transform.SetParent(shootGunPivot);
                    currentGun.transform.localPosition = Vector3.zero;
                    currentGun.transform.localRotation = Quaternion.Euler(Vector3.zero);

                    rig.transform.Find("Aim").GetComponent<MultiAimConstraint>().weight = 1.0f;
                    rig.transform.Find("Body").GetComponent<MultiAimConstraint>().weight = 1.0f;
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

        // [�� & ��Ŭ��]
        if (currentWeapon == Weapon.Gun && Input.GetMouseButton(0))
        {
            if (isFirstPerson || isThirdPerson)
            {
                if (currentGun != null && currentGun.GetComponent<Gun>().currentMag > 0)
                {
                    currentGun.GetComponent<Gun>().PlayerShoot();
                    GunRecoil();
                }
            }
        }

        // [����ź & ��Ŭ��]
        if (currentWeapon == Weapon.Grenade && cm.isGround && !isReloading)
        {
            if (Input.GetMouseButtonDown(1))
            {
                isThirdPerson = true;
                zoomControl.Third_ZoomIn();
                // ���� On
                GetComponent<DrawProjection>().drawProjection = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                isThirdPerson = false;
                zoomControl.Third_ZoomOut();
                // ���� Off
                GetComponent<DrawProjection>().drawProjection = false;
            }
        }

        // [����ź & ��Ŭ��]
        if (currentWeapon == Weapon.Grenade && Input.GetMouseButtonDown(0))
        {
            if (isThirdPerson && canThrow)
            {
                canThrow = false;
                StartCoroutine(ThrowGrenade());
                // ���� Off
                GetComponent<DrawProjection>().drawProjection = false;
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
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f)); // ȭ�� �߾� (ũ�ν���� ��ġ)�� Ray ���
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 100f, ~(1 << LayerMask.NameToLayer("Player"))))
        {
            aimTarget.transform.position = raycastHit.point;
        }

        if (!isFirstPerson && !isThirdPerson)
        {
            // body weight = 0
            rig.transform.Find("Body").GetComponent<MultiAimConstraint>().weight = 0.0f;
        }
    }

    private IEnumerator Reload_co()
    {
        if (isFirstPerson)
        {
            isFirstPerson = false;
            cm.currentSpeed = cm.walkSpeed;
            zoomControl.First_ZoomOut();

            rig.transform.Find("Aim").GetComponent<MultiAimConstraint>().weight = 0f;
            rig.transform.Find("Body").GetComponent<MultiAimConstraint>().weight = 0f;
        }
        else if (isThirdPerson)
        {
            isThirdPerson = false;
            cm.currentSpeed = cm.walkSpeed;
            zoomControl.Third_ZoomOut();

            rig.transform.Find("Aim").GetComponent<MultiAimConstraint>().weight = 0f;
            rig.transform.Find("LeftHand").GetComponent<TwoBoneIKConstraint>().weight = 1.0f;

            currentGun.transform.SetParent(holdGunPivot);
            currentGun.transform.localPosition = Vector3.zero;
            currentGun.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        rig.GetComponent<Rig>().weight = 0f;

        animator.SetTrigger("Reload");
        currentGun.GetComponent<Gun>().PlayerReload();

        yield return new WaitForSeconds(1.725f);
        rig.GetComponent<Rig>().weight = 1.0f;

        isReloading = false;
    }

    public void EquipGun(GameObject gun)
    {
        currentGun = gun;
    }

    private IEnumerator UnEquipGun_co()
    {
        animator.SetTrigger("UnEquip");
        rig.GetComponent<Rig>().weight = 0f;

        yield return new WaitForSeconds(0.7f);

        if (currentGun != null)
        {
            currentGun.transform.SetParent(backGunPivot);
            currentGun.transform.localPosition = Vector3.zero;
            currentGun.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    public void GunRecoil()
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

    private IEnumerator ThrowGrenade() // ����ź ������
    {
        Vector3 direction = throwDirection * throwPower; // ���� �̸� ����

        animator.SetTrigger("ThrowGrenade"); // �ִϸ��̼� ���

        yield return new WaitForSeconds(0.55f);

        GameObject currentGrenade = Instantiate(testGrenadePrefab, grenadePivot.position, Quaternion.identity); // ����ź ����
        currentGrenade.GetComponent<Rigidbody>().velocity = direction; // �������� ������
        currentGrenade.GetComponent<Grenade>().StartTimer(); // ����ź Ÿ�̸� ����

        yield return new WaitForSeconds(1.0f);

        canThrow = true;
    }
}
