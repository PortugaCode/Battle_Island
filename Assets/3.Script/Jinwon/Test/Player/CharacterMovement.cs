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

        // [마우스 커서 고정]
        Cursor.lockState = CursorLockMode.Locked;

        // [이동 속도 초기화]
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
        #region 키보드 입력
        // [방향 입력]
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        // [속도 변경] - 걷기, 달리기
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

        // [점프]
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

        #region 마우스 입력
        // [마우스 입력]
        // 1. 우클릭 -> 타이머 시작
        // 2. 우클릭 해제 시 타이머에 따라
        // 살짝 눌렀으면 1인칭 진입
        // 원래 3인칭 상태였으면 3인칭 해제
        // 원래 1인칭 상태였으면 1인칭 해제
        // 3. 우클릭 한 채로 오래 있으면 3인칭 진입

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
                    // 3인칭 진입
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
                    // 1인칭 해제
                    isFirstPerson = false;
                    currentSpeed = walkSpeed;
                    zoomControl.First_ZoomOut();
                    return;
                }

                if (isThirdPerson)
                {
                    // 3인칭 해제
                    isThirdPerson = false;
                    currentSpeed = walkSpeed;
                    zoomControl.Third_ZoomOut();
                    return;
                }

                if (!isFirstPerson && !isThirdPerson && clickTimer < thirdPersonEnterTime)
                {
                    // 1인칭 진입
                    isFirstPerson = true;
                    currentSpeed = firstPersonSpeed;
                    zoomControl.First_ZoomIn();
                    return;
                }
            }
        }
        #endregion

        // [플레이어 회전]
        transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

        
    }

    private void Move()
    {
        // [속도 변경] - 횡이동 시 속도
        float strafeSpeed;
        if (currentSpeed > walkSpeed)
        {
            strafeSpeed = walkSpeed;
        }
        else
        {
            strafeSpeed = currentSpeed;
        }

        // [속도 변경] - 점프 시 속도
        if (!isGround)
        {
            currentSpeed = walkSpeed;
        }

        // [이동]
        Vector3 targetDirection = transform.forward * z * currentSpeed + transform.right * x * strafeSpeed;
        rb.velocity = new Vector3(targetDirection.x, rb.velocity.y, targetDirection.z);
    }

    private void GroundCheck()
    {
        // [플레이어가 땅에 닿아있는지 체크]
        isGround = Physics.OverlapSphere(transform.position, 0.25f, groundLayer).Length > 0;
    }
}
