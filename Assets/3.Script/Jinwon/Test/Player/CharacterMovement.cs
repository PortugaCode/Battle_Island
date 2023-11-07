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

        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GetInput()
    {
        #region 키보드 입력
        // [키보드 입력]
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        #endregion

        #region 마우스 입력
        // [마우스 입력]
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
        #endregion

        // [회전]
        transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

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
    }

    private void Move()
    {
        // [속도 변경] - 횡 이동
        float strafeSpeed;
        if (currentSpeed > walkSpeed)
        {
            strafeSpeed = walkSpeed;
        }
        else
        {
            strafeSpeed = currentSpeed;
        }

        // [이동]
        Vector3 targetDirection = transform.forward * z * currentSpeed + transform.right * x * strafeSpeed;
        rb.velocity = targetDirection;
    }
}
