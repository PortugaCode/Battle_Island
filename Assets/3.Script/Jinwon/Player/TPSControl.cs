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
    private bool isRun = false;
    private float x;
    private float z;
    private Vector3 direction;

    [Header("Jump")]
    public LayerMask groundLayer;
    public float jumpForce = 2.0f;
    private bool isGround = true;

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
    private bool canThrow = true;

    [Header("Car")]
    private GameObject nearCar;
    private bool isCarEntered = false;

    // Weapon Status
    private Weapon currentWeapon = Weapon.None;

    // Components
    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        TryGetComponent(out rb);
        TryGetComponent(out animator);

        Cursor.visible = false; // 마우스 커서 비활성화
        currentSpeed = walkSpeed; // 이동속도 초기화
    }

    private void Update()
    {
        if (!isCarEntered)
        {
            GetMouseInput(); // 마우스 입력
            GetKeyboardInput(); // 키보드 입력
            PlayerMove(); // 이동
            ZoomCheck(); // 줌
            CheckCar(); // 차량 체크
            GroundCheck(); // 땅 체크

            if (Input.GetKeyDown(KeyCode.Keypad1)) // 총 장착 테스트
            {
                EquipGun();
                currentWeapon = Weapon.Gun;
            }

            if (Input.GetKeyDown(KeyCode.Keypad2)) // 수류탄 장착 테스트
            {
                currentWeapon = Weapon.Grenade;
            }

            if (Input.GetKeyDown(KeyCode.Return)) // 승차
            {
                EnterCar();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return)) // 하차
            {
                ExitCar();
            }
        }
    }

    private void GetMouseInput()
    {
        if (currentWeapon == Weapon.Gun && currentGun != null)
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
                currentGun.GetComponent<TestRifle>().Shoot();
            }
        }
        else if (currentWeapon == Weapon.Grenade)
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
                    GetComponent<DrawProjection>().drawProjection = false;
                    isAim = false;
                }
            }

            if (canThrow)
            {
                if (isAim && Input.GetMouseButton(0)) // 수류탄 조준
                {
                    GetComponent<DrawProjection>().drawProjection = true;
                }

                if (isAim && Input.GetMouseButtonUp(0)) // 수류탄 투척
                {
                    canThrow = false;
                    GetComponent<DrawProjection>().drawProjection = false;
                    StartCoroutine(ThrowGrenade());
                }
            }
        }

        // 마우스 회전 각도에 따라 궤적 변경
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
        throwDirection = transform.up * upPower + transform.forward; // 마우스 회전에 따라 수류탄 투척 방향 결정
    }

    private void GetKeyboardInput()
    {
        //x = Input.GetAxis("Horizontal"); // x축 입력
        //z = Input.GetAxis("Vertical"); // z축 입력

        if (Input.GetKey(KeyCode.LeftShift)) // LeftShift 입력 시 달리기
        {
            isRun = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift)) // 달리기 취소
        {
            isRun = false;
        }

        if (Input.GetKey(KeyCode.W)) // 앞으로 갈 때
        {
            if (isRun == true)
            {
                if (z < 2)
                {
                    z += Time.deltaTime * 2.0f;
                }

                if (z > 2) z = 2;
            }
            else if (isRun == false)
            {
                if (z < 1)
                {
                    z += Time.deltaTime * 2.0f;
                }

                if (z > 1) z -= Time.deltaTime * 2.0f;
            }
        }
        else if (Input.GetKey(KeyCode.S)) // 뒤로 갈 때
        {
            if (z > -1)
            {
                z -= Time.deltaTime * 2.0f;
            }

            if (z < -1) z = -1;
        }
        else // z축 입력 없을 때 0으로
        {
            if (z > 0)
            {
                z -= Time.deltaTime * 10.0f;
            }
            else if (z < 0)
            {
                z += Time.deltaTime * 10.0f;
            }

            if (Mathf.Abs(z) <= 0.001f) // 0으로 보정
            {
                z = 0;
            }
        }

        if (Input.GetKey(KeyCode.D)) // 오른쪽으로 갈 때
        {
            if (x < 1)
            {
                x += Time.deltaTime * 2.0f;
            }

            if (x > 1) x -= Time.deltaTime * 2.0f;
        }
        else if (Input.GetKey(KeyCode.A)) // 왼쪽으로 갈 때
        {
            if (x > -1)
            {
                x -= Time.deltaTime * 2.0f;
            }

            if (x < -1) x = -1;
        }
        else // x축 입력 없을 때 0으로
        {
            if (x > 0)
            {
                x -= Time.deltaTime * 10.0f;
            }
            else if (x < 0)
            {
                x += Time.deltaTime * 10.0f;
            }

            if (Mathf.Abs(x) <= 0.001f) // 0으로 보정
            {
                x = 0;
            }
        }

        if (isGround && Input.GetKeyDown(KeyCode.Space)) // 점프
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        animator.SetFloat("MoveSpeedX", x); // x축 입력값 블렌드 트리에 적용
        animator.SetFloat("MoveSpeedZ", z); // z축 입력값 블렌드 트리에 적용

        direction = new Vector3(x, 0, z).normalized;
    }

    private void PlayerMove()
    {
        //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;

        // 카메라 바라보는 방향으로 캐릭터 회전
        float cameraAngle = mainCamera.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cameraAngle, ref turnSmoothVelocity, turnSmoothTime); // 부드러운 회전 적용
        transform.rotation = Quaternion.Euler(0, smoothAngle, 0); // 플레이어 로테이션값 변경 (회전)

        if (isRun && z > 0) // 플레이어 속도 조정
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        // 캐릭터 이동
        if (direction.magnitude != 0f)
        {
            Vector3 moveDirectionZ = transform.forward * z;
            Vector3 moveDirectionX = transform.right * x;
            Vector3 moveDirection = moveDirectionX + moveDirectionZ; // x축, z축 보정

            rb.MovePosition(rb.position + moveDirection.normalized * currentSpeed * Time.deltaTime); // 플레이어 이동
        }
    }

    private void GroundCheck()
    {
        isGround = Physics.OverlapSphere(transform.position, 0.5f, groundLayer).Length > 0;
    }

    private void ZoomCheck()
    {
        if (isAim && !aimCamera.gameObject.activeSelf) // Zoom In
        {
            animator.SetTrigger("Aim"); // 줌 인 애니메이션
            aimCamera.m_XAxis.Value = normalCamera.m_XAxis.Value; // 두 카메라 x값 동기화
            aimCamera.m_YAxis.Value = normalCamera.m_YAxis.Value; // 두 카메라 y값 동기화
            aimCamera.gameObject.SetActive(true); // 에임 카메라 On
            UIManager.instance.Crosshair(true); // 크로스헤어 On
            currentSpeed = aimWalkSpeed; // 플레이어 속도 조정
        }
        else if (!isAim && aimCamera.gameObject.activeSelf) // Zoom Out
        {
            animator.SetTrigger("UnAim"); // 줌 아웃 애니메이션
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

    private IEnumerator ThrowGrenade()
    {
        Vector3 direction = throwDirection * throwPower; // 방향 미리 지정

        animator.SetTrigger("ThrowGrenade");

        yield return new WaitForSeconds(1.8f);

        GameObject currentGrenade = Instantiate(testGrenadePrefab, grenadePivot.position, Quaternion.identity);
        currentGrenade.GetComponent<Rigidbody>().velocity = direction;
        currentGrenade.GetComponent<Grenade>().StartTimer();

        yield return new WaitForSeconds(0.5f);

        canThrow = true;
    }

    private void GetItemAround() // 플레이어 주변 아이템 확인
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3.0f);

        foreach (Collider c in colliders)
        {
            // 주변 아이템들 쭉 탐색
        }
    }

    private void CheckCar() // 플레이어 주변 차량 확인
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3.0f);

        foreach (Collider c in colliders)
        {
            if (c.GetComponent<CarControl>())
            {
                nearCar = c.gameObject;
                return;
            }

            /*if (c.CompareTag("Car"))
            {
                nearCar = c.gameObject;
                return;
            }*/
        }
    }

    private void EnterCar() // 승차
    {
        if (nearCar == null)
        {
            return;
        }

        isCarEntered = true;
        transform.GetChild(7).gameObject.SetActive(false);
        nearCar.GetComponent<CarControl>().EnterCar();
    }

    private void ExitCar() // 하차
    {
        if (nearCar == null)
        {
            return;
        }

        isCarEntered = false;
        nearCar.GetComponent<CarControl>().ExitCar();
        transform.GetChild(7).gameObject.SetActive(true);
        transform.position = nearCar.GetComponent<CarControl>().playerPosition.position;
        transform.forward = nearCar.GetComponent<CarControl>().playerPosition.forward;
    }
}
