using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarControl : MonoBehaviour
{
    public Text directionText;

    private Rigidbody rb;

    private float turnSmoothVelocity;

    [SerializeField] private Transform handle;

    private float x; // �¿� (����)
    private float z; // �յ� (�ӷ�)

    private void Awake()
    {
        TryGetComponent(out rb);
    }

    private void Update()
    {
        directionText.text = x.ToString();
    }

    private void FixedUpdate()
    {
        GetKeyboardInput();
        CarMove();
    }

    private void GetKeyboardInput()
    {
        // �յ� �Է�
        if (Input.GetKey(KeyCode.W)) // ������ �� ��
        {
            if (z < 50)
            {
                z += Time.deltaTime * 5.0f;
            }

            if (z > 50) z = 50;
        }
        else if (Input.GetKey(KeyCode.S)) // �ڷ� �� ��
        {
            if (z > -10)
            {
                z -= Time.deltaTime * 5.0f;
            }

            if (z < -10) z = -10;
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

        float xLimit = 1.0f;

        // �¿� �Է�
        if (Input.GetKey(KeyCode.D)) // ���������� �� ��
        {
            if (x < 0)
            {
                x = 0;
            }

            if (x < xLimit)
            {
                x += Time.deltaTime * 30.0f;
            }

            if (x > xLimit) x = xLimit;
        }
        else if (Input.GetKey(KeyCode.A)) // �������� �� ��
        {
            if (x > 0)
            {
                x = 0;
            }

            if (x > -xLimit)
            {
                x -= Time.deltaTime * 30.0f;
            }

            if (x < -xLimit) x = -xLimit;
        }
        else // x�� �Է� ���� �� 0����
        {
            if (x > 0)
            {
                x -= Time.deltaTime * 30.0f;
            }
            else if (x < 0)
            {
                x += Time.deltaTime * 30.0f;
            }

            if (Mathf.Abs(x) <= 0.01f) // 0���� ����
            {
                x = 0;
            }
        }

        handle.localRotation = Quaternion.Euler(new Vector3(0, x, 0));
    }

    private void CarMove()
    {
        Vector3 moveDirection = handle.forward * z;

        if (Mathf.Abs(z) > 0)
        {
            transform.forward = handle.forward;

            rb.MovePosition(rb.position + moveDirection * Time.deltaTime); // ���� �̵�
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 moveDirection = handle.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, moveDirection);
    }
}
