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

        Cursor.visible = false; // 마우스 커서 비활성화
        currentSpeed = walkSpeed; // 이동속도 초기화
    }

    private void Update()
    {
        GetMouseInput(); // 마우스 입력
        GetKeyboardInput(); // 키보드 입력
        Move(); // 이동
        ZoomCheck(); // 줌

        if (Input.GetKeyDown(KeyCode.Return)) // 총 장착 테스트용
        {
            EquipGun();
        }

        if (Input.GetKeyDown(KeyCode.Space)) // 수류탄 테스트용
        {
            ThrowGrenade();
        }
    }

    private void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(1)) // 조준
        {
            if (!isAim)
            {
                isAim = true;
            }
        }

        if (Input.GetMouseButtonUp(1)) // 조준 해제
        {
            if (isAim)
            {
                isAim = false;
            }
        }

        if (Input.GetMouseButton(1) && Input.GetMouseButton(0)) // 조준 중 발사
        {
            if (currentGun != null) // 장착된 총이 있다면 발사 메서드 호출
            {
                currentGun.GetComponent<TestRifle>().Shoot();
            }
        }

        Debug.Log(mainCamera.eulerAngles.x);

        // x가 30일때 upPower는 0.5f
        // x가 0일때 upPower는 1.0f
        // x가 330 (-30)일때 upPower는 1.5f

        float upPower = 1.5f;
        throwDirection = transform.up * upPower + transform.forward; // 마우스 회전에 따라 수류탄 투척 방향 결정
    }

    private void GetKeyboardInput()
    {
        x = Input.GetAxisRaw("Horizontal"); // x축 입력
        z = Input.GetAxisRaw("Vertical"); // z축 입력
        direction = new Vector3(x, 0, z).normalized;
    }

    private void Move()
    {
        //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;

        // 카메라 바라보는 방향으로 캐릭터 회전
        float cameraAngle = mainCamera.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cameraAngle, ref turnSmoothVelocity, turnSmoothTime); // 부드러운 회전 적용
        transform.rotation = Quaternion.Euler(0, smoothAngle, 0); // 플레이어 로테이션값 변경 (회전)

        // 캐릭터 이동
        if (direction.magnitude != 0f)
        {
            Vector3 moveDirectionZ = transform.forward * z;
            Vector3 moveDirectionX = transform.right * x;
            Vector3 moveDirection = moveDirectionX + moveDirectionZ; // x축, z축 보정

            rb.MovePosition(rb.position + moveDirection.normalized * currentSpeed * Time.deltaTime); // 플레이어 이동
        }
    }

    private void ZoomCheck()
    {
        if (!hasGun) // 총이 있는 상태에서만 줌 가능
        {
            return;
        }

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
        if (hasGun) // 총이 없는 상태에서만 장착 가능
        {
            return;
        }

        hasGun = true;
        currentGun = Instantiate(testGunPrefab, gunPivot.position, gunPivot.rotation); // 총 생성
        currentGun.transform.SetParent(gunPivot); // GunPivot 위치에 장착
    }

    private void ThrowGrenade()
    {
        GameObject currentGrenade = Instantiate(testGrenadePrefab, gunPivot.position, Quaternion.identity);
        currentGrenade.GetComponent<Rigidbody>().velocity = throwDirection * throwPower;
    }
}
