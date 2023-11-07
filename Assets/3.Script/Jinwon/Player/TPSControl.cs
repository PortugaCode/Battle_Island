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
    private float currentHealth = 0f; // 현재 체력
    private float maxHealth = 100.0f; // 최대 체력

    [Header("Move")]
    public float walkSpeed = 2.0f; // 걷기 속도
    public float runSpeed = 3.0f; // 달리기 속도
    public float aimWalkSpeed = 1.0f; // 조준 상태 걷기 속도
    private float currentSpeed; // 현재 속도
    private bool isRun = false; // 달리고 있는지 체크
    private float x; // x축 입력값
    private float z; // z축 입력값
    private Vector3 direction; // 이동할 방향

    [Header("Jump")]
    public LayerMask groundLayer; // 땅 체크할 레이어
    public float jumpForce = 2.0f; // 점프력
    private bool isGround = true; // 땅에 닿아있는지 체크

    [Header("Camera")]
    [SerializeField] private GameObject characterModel; // 캐릭터 모델링
    [SerializeField] private Transform mainCamera; // 메인카메라
    [SerializeField] private CinemachineFreeLook normalCamera; // 평상시 카메라
    [SerializeField] private CinemachineFreeLook aimCamera; // 3인칭 조준시 카메라
    public CinemachineVirtualCamera firstPersonCamera; // 1인칭 조준시 카메라
    private float clickTimer = 0f; // 조준 클릭 타이머
    private bool timerOn = false; // 타이머 시작 여부
    public bool isFirstPersonView = false; // 1인칭 시점 여부
    public bool isThirdPersonView = false; // 3인칭 시점 여부
    public float turnSmoothTime = 0.1f; // 회전 시간
    private float turnSmoothVelocity; // 회전 속도

    [Header("Gun")]
    public GameObject gunPivot; // 총 피벗
    [SerializeField] private GameObject testGunPrefab; // 테스트용 총 프리팹
    private bool hasGun = false; // 총 장착 여부
    private GameObject currentGun = null; // 현재 장착한 총

    [Header("Recoil")]
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;

    [Header("Throwable")]
    public Transform grenadePivot; // 수류탄 피벗
    [SerializeField] private GameObject testGrenadePrefab; // 수류탄 프리팹
    public float throwPower = 10.0f; // 던지는 힘
    public Vector3 throwDirection; // 던질 방향
    private bool canThrow = true; // 던지기 가능 여부

    [Header("Car")]
    private GameObject nearCar; // 주변 차
    private bool isCarEntered = false; // 차 탑승 여부

    [Header("Item")]
    private List<GameObject> nearItemList = new List<GameObject>(); // 주변 아이템 리스트
    public LayerMask itemLayer; // 아이템 체크할 레이어

    // Weapon Status
    private Weapon currentWeapon = Weapon.None; // 현재 장착중인 무기

    // Components
    private Rigidbody rb;
    private CapsuleCollider cc;
    private Animator animator;

    private void Awake()
    {
        TryGetComponent(out rb);
        TryGetComponent(out cc);
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

            if (Input.GetKeyDown(KeyCode.Tab)) // 인벤토리 on off 테스트
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
        // 마우스 회전 각도에 따라 수류탄 궤적 변경
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

        if (currentWeapon == Weapon.Gun) // 총 장착 시
        {
            if (currentGun != null && Input.GetMouseButton(0))
            {
                GunRecoil();
                currentGun.GetComponent<Gun>().Shoot(); // 발사
            }
        }
        else if (currentWeapon == Weapon.Grenade) // 수류탄 장착 시
        {
            if (canThrow && Input.GetMouseButtonDown(0))
            {
                canThrow = false;
                StartCoroutine(ThrowGrenade()); // 투척
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

            rb.velocity = moveDirection * currentSpeed;
            //rb.MovePosition(rb.position + moveDirection.normalized * currentSpeed * Time.deltaTime); // 플레이어 이동
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
            characterModel.SetActive(false); // 플레이어 모델링 Off
            cc.isTrigger = true; // isTrigger On
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY; // Y값 고정
            gunPivot.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); // 총 모델링 Off
            firstPersonCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = normalCamera.m_XAxis.Value; // 두 카메라 x값 동기화
            firstPersonCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value = 2.0f; // y값 초기화
            firstPersonCamera.gameObject.SetActive(true); // 1인칭 카메라 On

            // 크로스헤어 On
            if (currentGun.GetComponent<Gun>().gunType == GunType.Rifle)
            {
                UIManager.instance.FirstPersonRifleCrosshair(true);
            }
            else if (currentGun.GetComponent<Gun>().gunType == GunType.Sniper)
            {
                UIManager.instance.FirstPersonSniperCrosshair(true);
            }
            
            currentSpeed = aimWalkSpeed; // 플레이어 속도 조정
        }
    }

    private void First_ZoomOut()
    {
        if (firstPersonCamera.gameObject.activeSelf) // Zoom Out
        {
            characterModel.SetActive(true); // 플레이어 모델링 On
            cc.isTrigger = false; // isTrigger Off
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ; // Y값 고정 해제
            gunPivot.transform.GetChild(0).GetChild(0).gameObject.SetActive(true); // 총 모델링 On
            normalCamera.m_XAxis.Value = firstPersonCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value; // 두 카메라 x값 동기화
            normalCamera.m_YAxis.Value = 0.35f; // y값 초기화
            firstPersonCamera.gameObject.SetActive(false); // 1인칭 카메라 Off

            // 크로스헤어 Off
            if (currentGun.GetComponent<Gun>().gunType == GunType.Rifle)
            {
                UIManager.instance.FirstPersonRifleCrosshair(false);
            }
            else if (currentGun.GetComponent<Gun>().gunType == GunType.Sniper)
            {
                UIManager.instance.FirstPersonSniperCrosshair(false);
            }

            currentSpeed = walkSpeed; // 플레이어 속도 조정
        }
    }

    private void Third_ZoomIn()
    {
        if (currentWeapon == Weapon.Grenade) // 수류탄 장착 시
        {
            GetComponent<DrawProjection>().drawProjection = true; // 궤적 켜기
        }

        if (!aimCamera.gameObject.activeSelf) // Zoom In
        {
            animator.SetTrigger("Aim"); // 줌 인 애니메이션
            aimCamera.m_XAxis.Value = normalCamera.m_XAxis.Value; // 카메라 x값 동기화
            aimCamera.m_YAxis.Value = normalCamera.m_YAxis.Value; // 카메라 y값 동기화
            aimCamera.gameObject.SetActive(true); // 에임 카메라 On
            UIManager.instance.ThirdPersonCrosshair(true); // 크로스헤어 On
            currentSpeed = aimWalkSpeed; // 플레이어 속도 조정
        }
    }

    private void Third_ZoomOut()
    {
        if (currentWeapon == Weapon.Grenade)
        {
            GetComponent<DrawProjection>().drawProjection = false; // 궤적 끄기
        }

        if (aimCamera.gameObject.activeSelf) // Zoom Out
        {
            animator.SetTrigger("UnAim"); // 줌 아웃 애니메이션
            normalCamera.m_XAxis.Value = aimCamera.m_XAxis.Value; // 카메라 x값 동기화
            normalCamera.m_YAxis.Value = aimCamera.m_YAxis.Value; // 카메라 y값 동기화
            aimCamera.gameObject.SetActive(false); // 에임 카메라 Off
            UIManager.instance.ThirdPersonCrosshair(false); // 크로스헤어 Off
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
        animator.SetBool("HasGun", true);
        animator.SetTrigger("EquipGun");
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

        yield return new WaitForSeconds(0.55f);

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
            nearItemList.Add(c.gameObject); // 주변 아이템들 리스트에 추가
        }
    }

    private void CheckCar() // 플레이어 주변 차량 확인
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3.0f);

        foreach (Collider c in colliders)
        {
            if (c.CompareTag("Car"))
            {
                nearCar = c.gameObject; // 주변 차 할당
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

        characterModel.SetActive(false); // 플레이어 모델링 Off

        if (gunPivot.transform.childCount != 0)
        {
            gunPivot.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); // 총 모델링 Off
        }
        
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

        characterModel.SetActive(true); // 플레이어 모델링 On

        if (gunPivot.transform.childCount != 0)
        {
            gunPivot.transform.GetChild(0).GetChild(0).gameObject.SetActive(true); // 총 모델링 On
        }

        transform.position = nearCar.GetComponent<CarControl>().playerPosition.position;
        transform.forward = nearCar.GetComponent<CarControl>().playerPosition.forward;
    }

    private void GunRecoil()
    {
        if (isFirstPersonView)
        {
            CinemachinePOV pov = firstPersonCamera.GetCinemachineComponent<CinemachinePOV>();
            pov.m_HorizontalAxis.Value += Random.Range(-recoilX, recoilX); // x축 반동;
            pov.m_VerticalAxis.Value -= Random.Range(0, recoilY); // y축 반동
        }
        else if (isThirdPersonView)
        {
            aimCamera.m_XAxis.Value += Random.Range(-recoilX * 0.01f, recoilX * 0.01f); // x축 반동;
            aimCamera.m_YAxis.Value -= Random.Range(0, recoilY * 0.01f); // y축 반동;
        }

        //mainCamera.eulerAngles += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
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
