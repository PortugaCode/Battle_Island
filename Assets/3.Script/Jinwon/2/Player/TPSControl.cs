using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum Weapon
{
    None,
    Gun,
    Grenade
}

public class TPSControl : MonoBehaviour
{
    [Header("Move")]
    public float walkSpeed = 2.0f;
    public float runSpeed = 3.0f;
    public float aimWalkSpeed = 1.0f;
    private float currentSpeed;
    private float x;
    private float z;
    private Vector3 direction;

    [Header("Camera")]
    [SerializeField] private Transform mainCamera;
    [SerializeField] private CinemachineFreeLook normalCamera;
    [SerializeField] private CinemachineFreeLook aimCamera;
    private bool isAim = false;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    [Header("Gun")]
    public Transform gunPivot;
    [SerializeField] private GameObject testGunPrefab;
    private bool hasGun = false;
    private GameObject currentGun = null;

    [Header("Throwable")]
    public Transform grenadePivot;
    [SerializeField] private GameObject testGrenadePrefab;
    public float throwPower = 10.0f;
    public Vector3 throwDirection;

    // Status
    private Weapon currentWeapon = Weapon.None;

    // Components
    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        TryGetComponent(out rb);
        TryGetComponent(out animator);

        Cursor.visible = false; // ���콺 Ŀ�� ��Ȱ��ȭ
        currentSpeed = walkSpeed; // �̵��ӵ� �ʱ�ȭ
    }

    private void Update()
    {
        GetMouseInput(); // ���콺 �Է�
        GetKeyboardInput(); // Ű���� �Է�
        Move(); // �̵�
        ZoomCheck(); // ��

        if (Input.GetKeyDown(KeyCode.Keypad1)) // �� ���� �׽�Ʈ
        {
            EquipGun();
            currentWeapon = Weapon.Gun;
        }

        if (Input.GetKeyDown(KeyCode.Keypad2)) // ����ź ���� �׽�Ʈ
        {
            currentWeapon = Weapon.Grenade;
        }
    }

    private void GetMouseInput()
    {
        if (currentWeapon == Weapon.Gun && currentGun != null)
        {
            if (Input.GetMouseButtonDown(1)) // ����
            {
                if (!isAim)
                {
                    isAim = true;
                }
            }

            if (Input.GetMouseButtonUp(1)) // ���� ����
            {
                if (isAim)
                {
                    isAim = false;
                }
            }

            if (Input.GetMouseButton(1) && Input.GetMouseButton(0)) // ���� �� �߻�
            {
                currentGun.GetComponent<TestRifle>().Shoot();
            }
        }
        else if (currentWeapon == Weapon.Grenade)
        {
            if (Input.GetMouseButtonDown(1)) // ����
            {
                if (!isAim)
                {
                    isAim = true;
                }
            }

            if (Input.GetMouseButtonUp(1)) // ���� ����
            {
                if (isAim)
                {
                    GetComponent<DrawProjection>().drawProjection = false;
                    isAim = false;
                }
            }

            if (isAim && Input.GetMouseButton(0)) // ����ź ����
            {
                GetComponent<DrawProjection>().drawProjection = true;
            }

            if (isAim && Input.GetMouseButtonUp(0)) // ����ź ��ô
            {
                GetComponent<DrawProjection>().drawProjection = false;
                StartCoroutine(ThrowGrenade());
            }
        }

        // ���콺 ȸ�� ������ ���� ���� ����
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
    }

    private void GetKeyboardInput()
    {
        x = Input.GetAxis("Horizontal"); // x�� �Է�
        z = Input.GetAxis("Vertical"); // z�� �Է�

        animator.SetFloat("MoveSpeedX", x); // x�� �Է°� ���� Ʈ���� ����
        animator.SetFloat("MoveSpeedZ", z); // z�� �Է°� ���� Ʈ���� ����

        direction = new Vector3(x, 0, z).normalized;
    }

    private void Move()
    {
        //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;

        // ī�޶� �ٶ󺸴� �������� ĳ���� ȸ��
        float cameraAngle = mainCamera.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cameraAngle, ref turnSmoothVelocity, turnSmoothTime); // �ε巯�� ȸ�� ����
        transform.rotation = Quaternion.Euler(0, smoothAngle, 0); // �÷��̾� �����̼ǰ� ���� (ȸ��)

        // ĳ���� �̵�
        if (direction.magnitude != 0f)
        {
            Vector3 moveDirectionZ = transform.forward * z;
            Vector3 moveDirectionX = transform.right * x;
            Vector3 moveDirection = moveDirectionX + moveDirectionZ; // x��, z�� ����

            rb.MovePosition(rb.position + moveDirection.normalized * currentSpeed * Time.deltaTime); // �÷��̾� �̵�
        }
    }

    private void ZoomCheck()
    {
        if (isAim && !aimCamera.gameObject.activeSelf) // Zoom In
        {
            animator.SetTrigger("Aim"); // �� �� �ִϸ��̼�
            aimCamera.m_XAxis.Value = normalCamera.m_XAxis.Value; // �� ī�޶� x�� ����ȭ
            aimCamera.m_YAxis.Value = normalCamera.m_YAxis.Value; // �� ī�޶� y�� ����ȭ
            aimCamera.gameObject.SetActive(true); // ���� ī�޶� On
            UIManager.instance.Crosshair(true); // ũ�ν���� On
            currentSpeed = aimWalkSpeed; // �÷��̾� �ӵ� ����
        }
        else if (!isAim && aimCamera.gameObject.activeSelf) // Zoom Out
        {
            animator.SetTrigger("UnAim"); // �� �ƿ� �ִϸ��̼�
            normalCamera.m_XAxis.Value = aimCamera.m_XAxis.Value; // �� ī�޶� x�� ����ȭ
            normalCamera.m_YAxis.Value = aimCamera.m_YAxis.Value; // �� ī�޶� y�� ����ȭ
            aimCamera.gameObject.SetActive(false); // ���� ī�޶� Off
            UIManager.instance.Crosshair(false); // ũ�ν���� Off
            currentSpeed = walkSpeed; // �÷��̾� �ӵ� ����
        }
    }

    private void EquipGun()
    {
        if (hasGun) // ���� ���� ���¿����� ���� ����
        {
            return;
        }

        hasGun = true;
        currentGun = Instantiate(testGunPrefab, gunPivot.position, gunPivot.rotation); // �� ����
        currentGun.transform.SetParent(gunPivot); // GunPivot ��ġ�� ����
    }

    private IEnumerator ThrowGrenade()
    {
        animator.SetTrigger("ThrowGrenade");

        yield return new WaitForSeconds(1.8f);

        GameObject currentGrenade = Instantiate(testGrenadePrefab, grenadePivot.position, Quaternion.identity);
        currentGrenade.GetComponent<Rigidbody>().velocity = throwDirection * throwPower;
        currentGrenade.GetComponent<Grenade>().StartTimer();
    }
}
