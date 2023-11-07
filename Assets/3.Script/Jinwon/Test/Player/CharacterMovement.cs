using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // Components
    private Rigidbody rb;
    private ZoomControl zoomControl;

    // Movement
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
    public LayerMask groundLayer;
    private bool isGround = true;

    // Movement Bool
    private bool isRun = false;

    // Mouse Input
    private bool timerOn = false;
    private float clickTimer = 0f;

    // Zoom
    private float thirdPersonEnterTime = 0.25f;
    private bool isFirstPerson = false;
    private bool isThirdPerson = false;

    private void Awake()
    {
        TryGetComponent(out rb);
        TryGetComponent(out zoomControl);

        // [���콺 Ŀ�� ����]
        Cursor.lockState = CursorLockMode.Locked;

        // [�̵� �ӵ� �ʱ�ȭ]
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        GetInput();
        GroundCheck();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GetInput()
    {
        #region Ű���� �Է�
        // [���� �Է�]
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        // [�ӵ� ����] - �ȱ�, �޸���
        if (!isRun && Input.GetKey(KeyCode.LeftShift) && z > 0)
        {
            isRun = true;
            currentSpeed = runSpeed;
        }
        else if (isRun)
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
                    return;
                }

                if (isThirdPerson)
                {
                    // 3��Ī ����
                    isThirdPerson = false;
                    currentSpeed = walkSpeed;
                    zoomControl.Third_ZoomOut();
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
        #endregion

        // [�÷��̾� ȸ��]
        transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

        
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
        // [�÷��̾ ���� ����ִ��� üũ]
        isGround = Physics.OverlapSphere(transform.position, 0.25f, groundLayer).Length > 0;
    }
}
