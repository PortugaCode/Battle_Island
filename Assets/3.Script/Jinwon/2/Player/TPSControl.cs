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
    [SerializeField] private Transform gunPivot;
    [SerializeField] private GameObject testGunPrefab;
    private bool hasGun = false;
    private GameObject currentGun = null;

    private void Awake()
    {
        TryGetComponent(out rb);

        Cursor.visible = false;
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        GetMouseInput();
        GetKeyboardInput();
        Move();
        ZoomCheck();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            EquipGun();
        }
    }

    private void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(1)) // ����
        {
            isAim = true;
        }

        if (Input.GetMouseButtonUp(1)) // ���� ����
        {
            isAim = false;
        }

        if (Input.GetMouseButton(0)) // �߻�
        {
            if (currentGun != null)
            {
                currentGun.GetComponent<TestRifle>().Shoot();
            }
        }
    }

    private void GetKeyboardInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        direction = new Vector3(x, 0, z).normalized;
    }

    private void Move()
    {
        //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;

        // ī�޶� �ٶ󺸴� �������� ĳ���� ȸ��
        float cameraAngle = mainCamera.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cameraAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

        // ĳ���� �̵�
        if (direction.magnitude != 0f)
        {
            Vector3 moveDirectionZ = transform.forward * z;
            Vector3 moveDirectionX = transform.right * x;
            Vector3 moveDirection = moveDirectionX + moveDirectionZ;

            rb.MovePosition(rb.position + moveDirection.normalized * currentSpeed * Time.deltaTime); // �÷��̾� �̵�
        }
    }

    private void ZoomCheck()
    {
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
        if (hasGun)
        {
            return;
        }

        hasGun = true;
        currentGun = Instantiate(testGunPrefab, gunPivot.position, gunPivot.rotation);
        currentGun.transform.SetParent(gunPivot);
    }
}
