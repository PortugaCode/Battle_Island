using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterMovement : MonoBehaviour
{
    // Components
    private Rigidbody rb;
    private Animator animator;
    private ZoomControl zoomControl;

    // Movement
    [Header("Movement")]
    public bool canMove = true;
    private float x;
    private float z;
    private float currentSpeed;
    private float walkSpeed = 2.5f;
    private float runSpeed = 6.5f;
    private float firstPersonSpeed = 0.5f;
    private float thirdPersonSpeed = 1.0f;

    // Jump
    private float jumpForce = 5.0f;

    // Ground Check
    [Header("Ground Check")]
    public LayerMask playerLayer;
    private bool isGround = true;

    // Movement Bool
    private bool isRun = false;

    // Mouse Input
    private bool timerOn = false;
    private float clickTimer = 0f;

    // Zoom
    [Header("Zoom Status")]
    private float thirdPersonEnterTime = 0.25f;
    public bool isFirstPerson = false;
    public bool isThirdPerson = false;

    // Aim
    [Header("Aim")]
    [SerializeField] private GameObject aimTarget;
    [SerializeField] private GameObject rig;
    [SerializeField] private Transform holdGunPivot;
    [SerializeField] private Transform shootGunPivot;
    private bool lookAround = false;
    public float normalCamX;
    public float normalCamY;

    // Gun
    [Header("Gun")]
    [SerializeField] private GameObject gun;
    private bool hasGun = false;


    private void Awake()
    {
        TryGetComponent(out rb);
        TryGetComponent(out animator);
        TryGetComponent(out zoomControl);

        // [���콺 Ŀ�� ����]
        Cursor.lockState = CursorLockMode.Locked;

        // [�̵� �ӵ� �ʱ�ȭ]
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        if (canMove)
        {
            GetInput();
            GroundCheck();
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
    }

    private void GetInput()
    {
        #region Ű���� �Է�
        // [���� �Է�]
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        // [�ִϸ����� ����]
        animator.SetFloat("MoveSpeedX", x);
        animator.SetFloat("MoveSpeedZ", z * currentSpeed);

        // [�� ����]
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            hasGun = true;
            rig.GetComponent<Rig>().weight = 1.0f;
            animator.SetBool("HasGun", true);
            animator.SetTrigger("EquipGun");
        }

        // [������]
        if (hasGun && Input.GetKeyDown(KeyCode.R))
        {

        }

        // [�ѷ�����]
        if (!isFirstPerson && !isThirdPerson && !lookAround && Input.GetKeyDown(KeyCode.LeftAlt))
        {
            zoomControl.StartLookAround();
            lookAround = true;
        }
        if (lookAround && Input.GetKeyUp(KeyCode.LeftAlt))
        {
            zoomControl.EndLookAround();
            lookAround = false;
        }

        // [�ӵ� ����] - �ȱ�, �޸���
        if (!isRun && Input.GetKey(KeyCode.LeftShift) && z > 0)
        {
            isRun = true;
            currentSpeed = runSpeed;
        }
        
        if (isRun && Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
            currentSpeed = walkSpeed;
        }

        // [����]
        if (isGround && Input.GetKeyDown(KeyCode.Space))
        {
            timerOn = false;
            clickTimer = 0;

            if (isFirstPerson)
            {
                isFirstPerson = false;
                currentSpeed = walkSpeed;
                zoomControl.First_ZoomOut();
            }
            else if (isThirdPerson)
            {
                isThirdPerson = false;
                currentSpeed = walkSpeed;
                zoomControl.Third_ZoomOut();
            }

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        #endregion

        #region ���콺 �Է�
        // [���콺 �Է�]
        // 1. ��Ŭ�� -> Ÿ�̸� ����
        // 2. ��Ŭ�� ���� �� Ÿ�̸ӿ� ����
        // ��¦ �������� 1��Ī ����
        // ���� 3��Ī ���¿����� 3��Ī ����
        // ���� 1��Ī ���¿����� 1��Ī ����
        // 3. ��Ŭ�� �� ä�� ���� ������ 3��Ī ����

        // [��Ŭ��]
        if (isGround)
        {
            if (Input.GetMouseButtonDown(1))
            {
                timerOn = true;
            }

            if (timerOn)
            {
                clickTimer += Time.deltaTime;
            }

            if (clickTimer >= thirdPersonEnterTime)
            {
                timerOn = false;
                clickTimer = 0;

                if (!isFirstPerson && !isThirdPerson)
                {
                    // 3��Ī ����
                    isThirdPerson = true;
                    currentSpeed = thirdPersonSpeed;
                    zoomControl.Third_ZoomIn();

                    rig.transform.Find("Aim").GetComponent<MultiAimConstraint>().weight = 1.0f;
                    rig.transform.Find("Body").GetComponent<MultiAimConstraint>().weight = 1.0f;

                    gun.transform.SetParent(shootGunPivot);
                    gun.transform.localPosition = Vector3.zero;
                    gun.transform.localRotation = Quaternion.Euler(Vector3.zero);
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                timerOn = false;
                clickTimer = 0;

                if (isFirstPerson)
                {
                    // 1��Ī ����
                    isFirstPerson = false;
                    currentSpeed = walkSpeed;
                    zoomControl.First_ZoomOut();

                    rig.transform.Find("Aim").GetComponent<MultiAimConstraint>().weight = 0f;
                    rig.transform.Find("Body").GetComponent<MultiAimConstraint>().weight = 0f;

                    return;
                }

                if (isThirdPerson)
                {
                    // 3��Ī ����
                    isThirdPerson = false;
                    currentSpeed = walkSpeed;
                    zoomControl.Third_ZoomOut();

                    rig.transform.Find("Aim").GetComponent<MultiAimConstraint>().weight = 0f;
                    rig.transform.Find("LeftHand").GetComponent<TwoBoneIKConstraint>().weight = 1.0f;

                    gun.transform.SetParent(holdGunPivot);
                    gun.transform.localPosition = Vector3.zero;
                    gun.transform.localRotation = Quaternion.Euler(Vector3.zero);

                    return;
                }

                if (!isFirstPerson && !isThirdPerson && clickTimer < thirdPersonEnterTime)
                {
                    // 1��Ī ����
                    isFirstPerson = true;
                    currentSpeed = firstPersonSpeed;
                    zoomControl.First_ZoomIn();
                    return;
                }
            }
        }

        // [��Ŭ��]
        if (Input.GetMouseButton(0))
        {
            if (isThirdPerson)
            {
                GetComponent<ShootTest>().Shoot();
            }
        }

        // [Aim ����]
        if (isFirstPerson || isThirdPerson)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f)); // ȭ�� �߾� (ũ�ν���� ��ġ)�� Ray ���
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 100f))
            {
                aimTarget.transform.position = raycastHit.point;
            }
        }
        else
        {
            // body weight = 0
            rig.transform.Find("Body").GetComponent<MultiAimConstraint>().weight = 0.0f;
        }

        #endregion

        // [�÷��̾� ȸ��]
        if (!lookAround)
        {
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        }
    }

    private void Move()
    {
        // [�ӵ� ����] - Ⱦ�̵� �� �ӵ�
        float strafeSpeed;
        if (currentSpeed > walkSpeed)
        {
            strafeSpeed = walkSpeed;
        }
        else
        {
            strafeSpeed = currentSpeed;
        }

        // [�ӵ� ����] - ���� �� �ӵ�
        if (!isGround)
        {
            currentSpeed = walkSpeed;
        }

        // [�̵�]
        Vector3 targetDirection = transform.forward * z * currentSpeed + transform.right * x * strafeSpeed;
        rb.velocity = new Vector3(targetDirection.x, rb.velocity.y, targetDirection.z);
    }

    private void GroundCheck()
    {
        // [�÷��̾� ���̾�� ����]
        int layerMask = ~playerLayer.value;

        // [�÷��̾ ���� ����ִ��� üũ]
        isGround = Physics.OverlapSphere(transform.position, 0.2f, layerMask).Length > 0;
    }
}
