using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TPSControl : MonoBehaviour
{
    [Header("Move")]
    private CharacterController characterController;
    public float moveSpeed = 2.0f;
    private float x;
    private float z;
    private Vector3 direction;

    [Header("Camera")]
    [SerializeField] private Transform mainCamera;
    [SerializeField] private CinemachineFreeLook normalCamera;
    [SerializeField] private CinemachineFreeLook aimCamera;
    private bool isAim = false;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private void Awake()
    {
        TryGetComponent(out characterController);
    }

    private void Update()
    {
        GetMouseInput();
        GetKeyboardInput();
        Move();
        ChangeCamera();
    }

    private void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isAim = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isAim = false;
        }
    }

    private void GetKeyboardInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        direction = new Vector3(x, 0, z).normalized;
    }

    private void Move()
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;

        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        if (direction.magnitude != 0f)
        {
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            characterController.Move(moveDirection.normalized * moveSpeed * Time.deltaTime); // �÷��̾� �̵�
        }

        if (direction.magnitude == 0f || (z > 0 && x == 0))
        {
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }

    private void ChangeCamera()
    {
        if (isAim && !aimCamera.gameObject.activeSelf)
        {
            aimCamera.m_XAxis.Value = normalCamera.m_XAxis.Value; // �� ī�޶� x�� ����ȭ
            aimCamera.m_YAxis.Value = normalCamera.m_YAxis.Value; // �� ī�޶� y�� ����ȭ
            aimCamera.gameObject.SetActive(true); // ���� ī�޶� On
            UIManager.instance.Crosshair(true); // ũ�ν���� On
        }
        else if (!isAim && aimCamera.gameObject.activeSelf)
        {
            normalCamera.m_XAxis.Value = aimCamera.m_XAxis.Value; // �� ī�޶� x�� ����ȭ
            normalCamera.m_YAxis.Value = aimCamera.m_YAxis.Value; // �� ī�޶� y�� ����ȭ
            aimCamera.gameObject.SetActive(false); // ���� ī�޶� Off
            UIManager.instance.Crosshair(false); // ũ�ν���� Off
        }
    }
}
