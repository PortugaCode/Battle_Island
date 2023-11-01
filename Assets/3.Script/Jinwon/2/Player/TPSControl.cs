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
        if (Input.GetMouseButtonDown(1)) // 조준
        {
            isAim = true;
        }

        if (Input.GetMouseButtonUp(1)) // 조준 해제
        {
            isAim = false;
        }

        if (Input.GetMouseButton(0)) // 발사
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

        // 카메라 바라보는 방향으로 캐릭터 회전
        float cameraAngle = mainCamera.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cameraAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

        // 캐릭터 이동
        if (direction.magnitude != 0f)
        {
            Vector3 moveDirectionZ = transform.forward * z;
            Vector3 moveDirectionX = transform.right * x;
            Vector3 moveDirection = moveDirectionX + moveDirectionZ;

            rb.MovePosition(rb.position + moveDirection.normalized * currentSpeed * Time.deltaTime); // 플레이어 이동
        }
    }

    private void ZoomCheck()
    {
        if (isAim && !aimCamera.gameObject.activeSelf) // Zoom In
        {
            aimCamera.m_XAxis.Value = normalCamera.m_XAxis.Value; // 두 카메라 x값 동기화
            aimCamera.m_YAxis.Value = normalCamera.m_YAxis.Value; // 두 카메라 y값 동기화
            aimCamera.gameObject.SetActive(true); // 에임 카메라 On
            UIManager.instance.Crosshair(true); // 크로스헤어 On
            currentSpeed = aimWalkSpeed; // 플레이어 속도 조정
        }
        else if (!isAim && aimCamera.gameObject.activeSelf) // Zoom Out
        {
            normalCamera.m_XAxis.Value = aimCamera.m_XAxis.Value; // 두 카메라 x값 동기화
            normalCamera.m_YAxis.Value = aimCamera.m_YAxis.Value; // 두 카메라 y값 동기화
            aimCamera.gameObject.SetActive(false); // 에임 카메라 Off
            UIManager.instance.Crosshair(false); // 크로스헤어 Off
            currentSpeed = walkSpeed; // 플레이어 속도 조정
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
