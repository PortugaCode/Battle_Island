using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarControl : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private Transform frontHandle;
    [SerializeField] private Transform backHandle;
    [SerializeField] private GameObject leftWheel;
    [SerializeField] private GameObject rightWheel;

    private float x; // �¿� (����)
    private float z; // �յ� (�ӷ�)

    private void Awake()
    {
        TryGetComponent(out rb);
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

            if (Mathf.Abs(z) < 0.1f) // 0���� ����
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
                x -= Time.deltaTime * 20.0f;
            }

            if (x < -xLimit) x = -xLimit;
        }
        else // x�� �Է� ���� �� 0����
        {
            if (x > 0)
            {
                x -= Time.deltaTime * 20.0f;
            }
            else if (x < 0)
            {
                x += Time.deltaTime * 20.0f;
            }

            if (Mathf.Abs(x) < 0.2f) // 0���� ����
            {
                x = 0;
            }
        }

        frontHandle.localRotation = Quaternion.Euler(new Vector3(0, x, 0));
        backHandle.localRotation = Quaternion.Euler(new Vector3(0, -x, 0));

        leftWheel.transform.localRotation = Quaternion.Euler(new Vector3(0, x * 30.0f, 0)); // ���� ���� ȸ��
        rightWheel.transform.localRotation = Quaternion.Euler(new Vector3(0, -180.0f + x * 30.0f, 0)); // ������ ���� ȸ��
    }

    private void CarMove()
    {
        Vector3 moveDirection = frontHandle.forward * z;
        Vector3 backDirection = backHandle.forward * z;

        if (z > 0)
        {
            transform.forward = frontHandle.forward;
            rb.MovePosition(rb.position + moveDirection * Time.deltaTime); // ���� �̵�
        }
        else if (z < 0)
        {
            transform.forward = backHandle.forward;
            rb.MovePosition(rb.position + backDirection * Time.deltaTime); // ���� �̵�
        }
    }
}
