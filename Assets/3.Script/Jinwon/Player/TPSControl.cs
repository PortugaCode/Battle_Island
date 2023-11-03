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

        Cursor.visible = false; // ���콺 Ŀ�� ��Ȱ��ȭ
        currentSpeed = walkSpeed; // �̵��ӵ� �ʱ�ȭ
    }

    private void Update()
    {
        if (!isCarEntered)
        {
            GetMouseInput(); // ���콺 �Է�
            GetKeyboardInput(); // Ű���� �Է�
            PlayerMove(); // �̵�
            ZoomCheck(); // ��
            CheckCar(); // ���� üũ
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
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return)) // ����
            {
                ExitCar();
            }
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

            if (canThrow)
            {
                if (isAim && Input.GetMouseButton(0)) // ����ź ����
                {
                    GetComponent<DrawProjection>().drawProjection = true;
                }

                if (isAim && Input.GetMouseButtonUp(0)) // ����ź ��ô
                {
                    canThrow = false;
                    GetComponent<DrawProjection>().drawProjection = false;
                    StartCoroutine(ThrowGrenade());
                }
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
        float cameraAngle = mainCamera.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cameraAngle, ref turnSmoothVelocity, turnSmoothTime); // �ε巯�� ȸ�� ����
        transform.rotation = Quaternion.Euler(0, smoothAngle, 0); // �÷��̾� �����̼ǰ� ���� (ȸ��)

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

    private void GroundCheck()
    {
        isGround = Physics.OverlapSphere(transform.position, 0.5f, groundLayer).Length > 0;
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
        Vector3 direction = throwDirection * throwPower; // ���� �̸� ����

        animator.SetTrigger("ThrowGrenade");

        yield return new WaitForSeconds(1.8f);

        GameObject currentGrenade = Instantiate(testGrenadePrefab, grenadePivot.position, Quaternion.identity);
        currentGrenade.GetComponent<Rigidbody>().velocity = direction;
        currentGrenade.GetComponent<Grenade>().StartTimer();

        yield return new WaitForSeconds(0.5f);

        canThrow = true;
    }

    private void GetItemAround() // �÷��̾� �ֺ� ������ Ȯ��
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3.0f);

        foreach (Collider c in colliders)
        {
            // �ֺ� �����۵� �� Ž��
        }
    }

    private void CheckCar() // �÷��̾� �ֺ� ���� Ȯ��
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

    private void EnterCar() // ����
    {
        if (nearCar == null)
        {
            return;
        }

        isCarEntered = true;
        transform.GetChild(7).gameObject.SetActive(false);
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
        transform.GetChild(7).gameObject.SetActive(true);
        transform.position = nearCar.GetComponent<CarControl>().playerPosition.position;
        transform.forward = nearCar.GetComponent<CarControl>().playerPosition.forward;
    }
}
