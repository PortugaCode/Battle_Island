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

        Cursor.visible = false; // ���콺 Ŀ�� ��Ȱ��ȭ
        currentSpeed = walkSpeed; // �̵��ӵ� �ʱ�ȭ
    }

    private void Update()
    {
        if (!isCarEntered)
        {
            GetMouseInput2(); // ���콺 �Է�
            GetKeyboardInput(); // Ű���� �Է�
            PlayerMove(); // �̵�
            GroundCheck(); // �� üũ

            if (Input.GetKeyDown(KeyCode.Keypad1)) // �� ���� �׽�Ʈ
            {
                EquipGun();
                currentWeapon = Weapon.Gun;
            }

            if (Input.GetKeyDown(KeyCode.Keypad2)) // ����ź ���� �׽�Ʈ
            {
                currentWeapon = Weapon.Grenade;
            }

            if (Input.GetKeyDown(KeyCode.Return)) // ����
            {
                EnterCar();
            }

            if (Input.GetKeyDown(KeyCode.Tab)) // �κ��丮 on off
            {
                GetItemAround(); // �ֺ� ������ Ž��
                UIManager.instance.ToggleInventory(nearItemList);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return)) // ����
            {
                ExitCar();
            }
        }
    }

    private void GetMouseInput2()
    {
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

        if (currentWeapon == Weapon.None)
        {
            return;
        }

        // 1. ��Ŭ�� -> Ÿ�̸� ����
        // 2. ��Ŭ�� ���� �� Ÿ�̸ӿ� ����
        // ��¦ �������� 1��Ī ����
        // ���� 3��Ī ���¿����� 3��Ī ����
        // ���� 1��Ī ���¿����� 1��Ī ����
        // 3. ��Ŭ�� �� ä�� ���� ������ 3��Ī ����

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
                // 3��Ī ����
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
                // 1��Ī ����
                isFirstPersonView = false;
                First_ZoomOut();
                return;
            }

            if (isThirdPersonView)
            {
                // 3��Ī ����
                isThirdPersonView = false;
                Third_ZoomOut();
                return;
            }

            if (!isFirstPersonView && !isThirdPersonView && clickTimer < 0.5f)
            {
                if (currentWeapon == Weapon.Gun)
                {
                    // 1��Ī ����
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
        //x = Input.GetAxis("Horizontal"); // x�� �Է�
        //z = Input.GetAxis("Vertical"); // z�� �Է�

        if (Input.GetKey(KeyCode.LeftShift)) // LeftShift �Է� �� �޸���
        {
            isRun = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift)) // �޸��� ���
        {
            isRun = false;
        }

        if (Input.GetKey(KeyCode.W)) // ������ �� ��
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
        else if (Input.GetKey(KeyCode.S)) // �ڷ� �� ��
        {
            if (z > -1)
            {
                z -= Time.deltaTime * 2.0f;
            }

            if (z < -1) z = -1;
        }
        else // z�� �Է� ���� �� 0����
        {
            if (z > 0)
            {
                z -= Time.deltaTime * 10.0f;
            }
            else if (z < 0)
            {
                z += Time.deltaTime * 10.0f;
            }

            if (Mathf.Abs(z) <= 0.001f) // 0���� ����
            {
                z = 0;
            }
        }

        if (Input.GetKey(KeyCode.D)) // ���������� �� ��
        {
            if (x < 1)
            {
                x += Time.deltaTime * 2.0f;
            }

            if (x > 1) x -= Time.deltaTime * 2.0f;
        }
        else if (Input.GetKey(KeyCode.A)) // �������� �� ��
        {
            if (x > -1)
            {
                x -= Time.deltaTime * 2.0f;
            }

            if (x < -1) x = -1;
        }
        else // x�� �Է� ���� �� 0����
        {
            if (x > 0)
            {
                x -= Time.deltaTime * 10.0f;
            }
            else if (x < 0)
            {
                x += Time.deltaTime * 10.0f;
            }

            if (Mathf.Abs(x) <= 0.001f) // 0���� ����
            {
                x = 0;
            }
        }

        if (isGround && Input.GetKeyDown(KeyCode.Space)) // ����
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        animator.SetFloat("MoveSpeedX", x); // x�� �Է°� ���� Ʈ���� ����
        animator.SetFloat("MoveSpeedZ", z); // z�� �Է°� ���� Ʈ���� ����

        direction = new Vector3(x, 0, z).normalized;
    }

    private void PlayerMove()
    {
        //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;

        // ī�޶� �ٶ󺸴� �������� ĳ���� ȸ��
        if (!isFirstPersonView)
        {
            float cameraAngle = mainCamera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cameraAngle, ref turnSmoothVelocity, turnSmoothTime); // �ε巯�� ȸ�� ����
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0); // �÷��̾� �����̼ǰ� ���� (ȸ��)
        }

        if (isRun && z > 0) // �÷��̾� �ӵ� ����
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        // ĳ���� �̵�
        if (direction.magnitude != 0f)
        {
            Vector3 moveDirectionZ = transform.forward * z;
            Vector3 moveDirectionX = transform.right * x;
            Vector3 moveDirection = moveDirectionX + moveDirectionZ; // x��, z�� ����

            rb.MovePosition(rb.position + moveDirection.normalized * currentSpeed * Time.deltaTime); // �÷��̾� �̵�
        }
    }

    private void GroundCheck() // ���� ����ִ��� üũ
    {
        isGround = Physics.OverlapSphere(transform.position, 0.5f, groundLayer).Length > 0; // Ground ���̾ ������ isGround = true;
    }

    private void First_ZoomIn()
    {
        if (!firstPersonCamera.gameObject.activeSelf) // Zoom In
        {
            characterModel.SetActive(false); // �𵨸� Off
            gunPivot.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            firstPersonCamera.gameObject.SetActive(true); // 1��Ī ī�޶� On
            UIManager.instance.Crosshair(true); // ũ�ν���� On
            currentSpeed = aimWalkSpeed; // �÷��̾� �ӵ� ����
        }
    }

    private void First_ZoomOut()
    {
        if (firstPersonCamera.gameObject.activeSelf) // Zoom Out
        {
            characterModel.SetActive(true); // �𵨸� On
            gunPivot.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            firstPersonCamera.gameObject.SetActive(false); // 1��Ī ī�޶� Off
            UIManager.instance.Crosshair(false); // ũ�ν���� Off
            currentSpeed = walkSpeed; // �÷��̾� �ӵ� ����
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
            animator.SetTrigger("Aim"); // �� �� �ִϸ��̼�
            aimCamera.m_XAxis.Value = normalCamera.m_XAxis.Value; // �� ī�޶� x�� ����ȭ
            aimCamera.m_YAxis.Value = normalCamera.m_YAxis.Value; // �� ī�޶� y�� ����ȭ
            aimCamera.gameObject.SetActive(true); // ���� ī�޶� On
            UIManager.instance.Crosshair(true); // ũ�ν���� On
            currentSpeed = aimWalkSpeed; // �÷��̾� �ӵ� ����
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
            animator.SetTrigger("UnAim"); // �� �ƿ� �ִϸ��̼�
            normalCamera.m_XAxis.Value = aimCamera.m_XAxis.Value; // �� ī�޶� x�� ����ȭ
            normalCamera.m_YAxis.Value = aimCamera.m_YAxis.Value; // �� ī�޶� y�� ����ȭ
            aimCamera.gameObject.SetActive(false); // ���� ī�޶� Off
            UIManager.instance.Crosshair(false); // ũ�ν���� Off
            currentSpeed = walkSpeed; // �÷��̾� �ӵ� ����
        }
    }

    private void EquipGun() // �� ����
    {
        if (hasGun) // ���� ���� ���¿����� ���� ����
        {
            return;
        }

        hasGun = true;
        currentGun = Instantiate(testGunPrefab, gunPivot.transform.position, gunPivot.transform.rotation); // �� ����
        currentGun.transform.SetParent(gunPivot.transform); // GunPivot ��ġ�� ����
    }

    private IEnumerator ThrowGrenade() // ����ź ������
    {
        if (gunPivot.transform.childCount != 0)
        {
            gunPivot.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); // �� �𵨸� Off
        }

        Vector3 direction = throwDirection * throwPower; // ���� �̸� ����

        animator.SetTrigger("ThrowGrenade"); // �ִϸ��̼� ���

        yield return new WaitForSeconds(0.525f);

        GameObject currentGrenade = Instantiate(testGrenadePrefab, grenadePivot.position, Quaternion.identity); // ����ź ����
        currentGrenade.GetComponent<Rigidbody>().velocity = direction; // �������� ������
        currentGrenade.GetComponent<Grenade>().StartTimer(); // ����ź Ÿ�̸� ����

        yield return new WaitForSeconds(1.0f);

        if (gunPivot.transform.childCount != 0)
        {
            gunPivot.transform.GetChild(0).GetChild(0).gameObject.SetActive(true); // �� �𵨸� On
        }

        canThrow = true;
    }

    private void GetItemAround() // �÷��̾� �ֺ� ������ Ȯ�� --> �κ��丮 ������ ȣ��
    {
        nearItemList.Clear();

        Collider[] colliders = Physics.OverlapSphere(transform.position, 3.0f, itemLayer); // �÷��̾� �ֺ��� item���̾� ������Ʈ�� ����

        foreach (Collider c in colliders)
        {
            nearItemList.Add(c.gameObject);
        }
    }

    private void CheckCar() // �÷��̾� �ֺ� ���� Ȯ��
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

    private void EnterCar() // ����
    {
        CheckCar(); // �ֺ� �� Ȯ��

        if (nearCar == null)
        {
            return;
        }

        isCarEntered = true;
        characterModel.SetActive(false);
        gunPivot.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        nearCar.GetComponent<CarControl>().EnterCar();
    }

    private void ExitCar() // ����
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

    private void Heal(float healAmount) // ü�� ȸ��
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
