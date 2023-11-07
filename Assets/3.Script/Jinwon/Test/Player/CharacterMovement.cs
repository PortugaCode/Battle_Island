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
        #region Ű���� �Է�
        // [Ű���� �Է�]
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        #endregion

        #region ���콺 �Է�
        // [���콺 �Է�]
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
        #endregion

        // [ȸ��]
        transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

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
    }

    private void Move()
    {
        // [�ӵ� ����] - Ⱦ �̵�
        float strafeSpeed;
        if (currentSpeed > walkSpeed)
        {
            strafeSpeed = walkSpeed;
        }
        else
        {
            strafeSpeed = currentSpeed;
        }

        // [�̵�]
        Vector3 targetDirection = transform.forward * z * currentSpeed + transform.right * x * strafeSpeed;
        rb.velocity = targetDirection;
    }
}
