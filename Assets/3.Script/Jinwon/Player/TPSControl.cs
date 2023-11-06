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
    // Player Status
    private float currentHealth = 0f;
    private float maxHealth = 100.0f;

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
    [SerializeField] private GameObject characterModel;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private CinemachineFreeLook normalCamera;
    [SerializeField] private CinemachineFreeLook aimCamera;
    [SerializeField] private CinemachineVirtualCamera firstPersonCamera;
    private float clickTimer = 0f;
    private bool timerOn = false;
    private bool isFirstPersonView = false;
    private bool isThirdPersonView = false;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    [Header("Gun")]
    public GameObject gunPivot;
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

    [Header("Item")]
    private List<GameObject> nearItemList = new List<GameObject>();
    public LayerMask itemLayer;

    // Weapon Status
    private Weapon currentWeapon = Weapon.None;

    // Components
    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        TryGetComponent(out rb);
        TryGetComponent(out animator);

        currentHealth = maxHealth;

        Cursor.visible = false; // 마우스 커서 비활성화
        currentSpeed = walkSpeed; // 이동속도 초기화
    }

    private void Update()
    {
        if (!isCarEntered)
        {
            GetMouseInput2(); // 마우스 입력
            GetKeyboardInput(); // 키보드 입력
            PlayerMove(); // 이동
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

            if (Input.GetKeyDown(KeyCode.Tab)) // 인벤토리 on off
            {
                GetItemAround(); // 주변 아이템 탐색
                UIManager.instance.ToggleInventory(nearItemList);
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

    private void GetMouseInput2()
    {
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

        if (currentWeapon == Weapon.None)
        {
            return;
        }

        // 1. 우클릭 -> 타이머 시작
        // 2. 우클릭 해제 시 타이머에 따라
        // 살짝 눌렀으면 1인칭 진입
        // 원래 3인칭 상태였으면 3인칭 해제
        // 원래 1인칭 상태였으면 1인칭 해제
        // 3. 우클릭 한 채로 오래 있으면 3인칭 진입

        if (Input.GetMouseButtonDown(1))
        {
            timerOn = true;
        }

        if (timerOn)
        {
            clickTimer += Time.deltaTime;
        }

        if (clickTimer >= 0.5f)
        {
            timerOn = false;
            clickTimer = 0;

            if (!isFirstPersonView && !isThirdPersonView)
            {
                // 3인칭 진입
                isThirdPersonView = true;
                Third_ZoomIn();
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            timerOn = false;
            clickTimer = 0;

            if (isFirstPersonView)
            {
                // 1인칭 해제
                isFirstPersonView = false;
                First_ZoomOut();
                return;
            }

            if (isThirdPersonView)
            {
                // 3인칭 해제
                isThirdPersonView = false;
                Third_ZoomOut();
                return;
            }

            if (!isFirstPersonView && !isThirdPersonView && clickTimer < 0.5f)
            {
                if (currentWeapon == Weapon.Gun)
                {
                    // 1인칭 진입
                    isFirstPersonView = true;
                    First_ZoomIn();
                    return;
                }
            }
        }

        if (currentWeapon == Weapon.Gun)
        {
            if (currentGun != null && Input.GetMouseButton(0))
            {
                currentGun.GetComponent<TestRifle>().Shoot();
            }
        }
        else if (currentWeapon == Weapon.Grenade)
        {
            if (canThrow && Input.GetMouseButtonDown(0))
            {
                canThrow = false;
                StartCoroutine(ThrowGrenade());
            }
        }
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
        if (!isFirstPersonView)
        {
            float cameraAngle = mainCamera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cameraAngle, ref turnSmoothVelocity, turnSmoothTime); // 부드러운 회전 적용
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0); // 플레이어 로테이션값 변경 (회전)
        }

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

    private void GroundCheck() // 땅에 닿아있는지 체크
    {
        isGround = Physics.OverlapSphere(transform.position, 0.5f, groundLayer).Length > 0; // Ground 레이어에 닿으면 isGround = true;
    }

    private void First_ZoomIn()
    {
        if (!firstPersonCamera.gameObject.activeSelf) // Zoom In
        {
            characterModel.SetActive(false); // 모델링 Off
            gunPivot.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            firstPersonCamera.gameObject.SetActive(true); // 1인칭 카메라 On
            UIManager.instance.Crosshair(true); // 크로스헤어 On
            currentSpeed = aimWalkSpeed; // 플레이어 속도 조정
        }
    }

    private void First_ZoomOut()
    {
        if (firstPersonCamera.gameObject.activeSelf) // Zoom Out
        {
            characterModel.SetActive(true); // 모델링 On
            gunPivot.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            firstPersonCamera.gameObject.SetActive(false); // 1인칭 카메라 Off
            UIManager.instance.Crosshair(false); // 크로스헤어 Off
            currentSpeed = walkSpeed; // 플레이어 속도 조정
        }
    }

    private void Third_ZoomIn()
    {
        if (currentWeapon == Weapon.Grenade)
        {
            GetComponent<DrawProjection>().drawProjection = true;
        }

        if (!aimCamera.gameObject.activeSelf) // Zoom In
        {
            animator.SetTrigger("Aim"); // 줌 인 애니메이션
            aimCamera.m_XAxis.Value = normalCamera.m_XAxis.Value; // 두 카메라 x값 동기화
            aimCamera.m_YAxis.Value = normalCamera.m_YAxis.Value; // 두 카메라 y값 동기화
            aimCamera.gameObject.SetActive(true); // 에임 카메라 On
            UIManager.instance.Crosshair(true); // 크로스헤어 On
            currentSpeed = aimWalkSpeed; // 플레이어 속도 조정
        }
    }

    private void Third_ZoomOut()
    {
        if (currentWeapon == Weapon.Grenade)
        {
            GetComponent<DrawProjection>().drawProjection = false;
        }

        if (aimCamera.gameObject.activeSelf) // Zoom Out
        {
            animator.SetTrigger("UnAim"); // 줌 아웃 애니메이션
            normalCamera.m_XAxis.Value = aimCamera.m_XAxis.Value; // 두 카메라 x값 동기화
            normalCamera.m_YAxis.Value = aimCamera.m_YAxis.Value; // 두 카메라 y값 동기화
            aimCamera.gameObject.SetActive(false); // 에임 카메라 Off
            UIManager.instance.Crosshair(false); // 크로스헤어 Off
            currentSpeed = walkSpeed; // 플레이어 속도 조정
        }
    }

    private void EquipGun() // 총 장착
    {
        if (hasGun) // 총이 없는 상태에서만 장착 가능
        {
            return;
        }

        hasGun = true;
        currentGun = Instantiate(testGunPrefab, gunPivot.transform.position, gunPivot.transform.rotation); // 총 생성
        currentGun.transform.SetParent(gunPivot.transform); // GunPivot 위치에 장착
    }

    private IEnumerator ThrowGrenade() // 수류탄 던지기
    {
        if (gunPivot.transform.childCount != 0)
        {
            gunPivot.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); // 총 모델링 Off
        }

        Vector3 direction = throwDirection * throwPower; // 방향 미리 지정

        animator.SetTrigger("ThrowGrenade"); // 애니메이션 재생

        yield return new WaitForSeconds(0.525f);

        GameObject currentGrenade = Instantiate(testGrenadePrefab, grenadePivot.position, Quaternion.identity); // 수류탄 생성
        currentGrenade.GetComponent<Rigidbody>().velocity = direction; // 방향으로 던지기
        currentGrenade.GetComponent<Grenade>().StartTimer(); // 수류탄 타이머 시작

        yield return new WaitForSeconds(1.0f);

        if (gunPivot.transform.childCount != 0)
        {
            gunPivot.transform.GetChild(0).GetChild(0).gameObject.SetActive(true); // 총 모델링 On
        }

        canThrow = true;
    }

    private void GetItemAround() // 플레이어 주변 아이템 확인 --> 인벤토리 열때만 호출
    {
        nearItemList.Clear();

        Collider[] colliders = Physics.OverlapSphere(transform.position, 3.0f, itemLayer); // 플레이어 주변의 item레이어 오브젝트들 검출

        foreach (Collider c in colliders)
        {
            nearItemList.Add(c.gameObject);
        }
    }

    private void CheckCar() // 플레이어 주변 차량 확인
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3.0f);

        foreach (Collider c in colliders)
        {
            if (c.CompareTag("Car"))
            {
                nearCar = c.gameObject;
                return;
            }
        }
    }

    private void EnterCar() // 승차
    {
        CheckCar(); // 주변 차 확인

        if (nearCar == null)
        {
            return;
        }

        isCarEntered = true;
        characterModel.SetActive(false);
        gunPivot.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
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
        characterModel.SetActive(true);
        gunPivot.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        transform.position = nearCar.GetComponent<CarControl>().playerPosition.position;
        transform.forward = nearCar.GetComponent<CarControl>().playerPosition.forward;
    }

    private void Heal(float healAmount) // 체력 회복
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
