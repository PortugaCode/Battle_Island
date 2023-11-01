using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TPSControl : MonoBehaviour
{
    [Header("Move")]
    private Rigidbody rb;
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
    [SerializeField] private GameObject testGrenadePrefab;
    public float throwPower = 10.0f;
    public Vector3 throwDirection;

    private void Awake()
    {
        TryGetComponent(out rb);

        Cursor.visible = false; // ���콺 Ŀ�� ��Ȱ��ȭ
        currentSpeed = walkSpeed; // �̵��ӵ� �ʱ�ȭ
    }

    private void Update()
    {
        GetMouseInput(); // ���콺 �Է�
        GetKeyboardInput(); // Ű���� �Է�
        Move(); // �̵�
        ZoomCheck(); // ��

        if (Input.GetKeyDown(KeyCode.Return)) // �� ���� �׽�Ʈ��
        {
            EquipGun();
        }

        if (Input.GetKeyDown(KeyCode.Space)) // ����ź �׽�Ʈ��
        {
            ThrowGrenade();
        }
    }

    private void GetMouseInput()
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
            if (currentGun != null) // ������ ���� �ִٸ� �߻� �޼��� ȣ��
            {
                currentGun.GetComponent<TestRifle>().Shoot();
            }
        }

        Debug.Log(mainCamera.eulerAngles.x);

        // x�� 30�϶� upPower�� 0.5f
        // x�� 0�϶� upPower�� 1.0f
        // x�� 330 (-30)�϶� upPower�� 1.5f

        float upPower = 1.5f;
        throwDirection = transform.up * upPower + transform.forward; // ���콺 ȸ���� ���� ����ź ��ô ���� ����
    }

    private void GetKeyboardInput()
    {
        x = Input.GetAxisRaw("Horizontal"); // x�� �Է�
        z = Input.GetAxisRaw("Vertical"); // z�� �Է�
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
        if (!hasGun) // ���� �ִ� ���¿����� �� ����
        {
            return;
        }

        if (isAim && !aimCamera.gameObject.activeSelf) // Zoom In
        {
            aimCamera.m_XAxis.Value = normalCamera.m_XAxis.Value; // �� ī�޶� x�� ����ȭ
            aimCamera.m_YAxis.Value = normalCamera.m_YAxis.Value; // �� ī�޶� y�� ����ȭ
            aimCamera.gameObject.SetActive(true); // ���� ī�޶� On
            UIManager.instance.Crosshair(true); // ũ�ν���� On
            currentSpeed = aimWalkSpeed; // �÷��̾� �ӵ� ����
        }
        else if (!isAim && aimCamera.gameObject.activeSelf) // Zoom Out
        {
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

    private void ThrowGrenade()
    {
        GameObject currentGrenade = Instantiate(testGrenadePrefab, gunPivot.position, Quaternion.identity);
        currentGrenade.GetComponent<Rigidbody>().velocity = throwDirection * throwPower;
    }
}
