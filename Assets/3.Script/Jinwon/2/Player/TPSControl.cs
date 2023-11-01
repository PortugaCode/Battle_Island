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

            characterController.Move(moveDirection.normalized * moveSpeed * Time.deltaTime); // 플레이어 이동
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
            aimCamera.m_XAxis.Value = normalCamera.m_XAxis.Value; // 두 카메라 x값 동기화
            aimCamera.m_YAxis.Value = normalCamera.m_YAxis.Value; // 두 카메라 y값 동기화
            aimCamera.gameObject.SetActive(true); // 에임 카메라 On
            UIManager.instance.Crosshair(true); // 크로스헤어 On
        }
        else if (!isAim && aimCamera.gameObject.activeSelf)
        {
            normalCamera.m_XAxis.Value = aimCamera.m_XAxis.Value; // 두 카메라 x값 동기화
            normalCamera.m_YAxis.Value = aimCamera.m_YAxis.Value; // 두 카메라 y값 동기화
            aimCamera.gameObject.SetActive(false); // 에임 카메라 Off
            UIManager.instance.Crosshair(false); // 크로스헤어 Off
        }
    }
}
