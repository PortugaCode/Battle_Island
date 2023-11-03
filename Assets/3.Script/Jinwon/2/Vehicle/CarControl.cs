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

    private float x; // 좌우 (각도)
    private float z; // 앞뒤 (속력)

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
        // 앞뒤 입력
        if (Input.GetKey(KeyCode.W)) // 앞으로 갈 때
        {
            if (z < 50)
            {
                z += Time.deltaTime * 5.0f;
            }

            if (z > 50) z = 50;
        }
        else if (Input.GetKey(KeyCode.S)) // 뒤로 갈 때
        {
            if (z > -10)
            {
                z -= Time.deltaTime * 5.0f;
            }

            if (z < -10) z = -10;
        }
        else // z축 입력 없을 때 0으로
        {
            if (z > 0)
            {
                z -= Time.deltaTime * 10.0f;
            }
            else if (z < 0)
            {
                z += Time.deltaTime * 10.0f;
            }

            if (Mathf.Abs(z) < 0.1f) // 0으로 보정
            {
                z = 0;
            }
        }

        float xLimit = 1.0f;

        // 좌우 입력
        if (Input.GetKey(KeyCode.D)) // 오른쪽으로 갈 때
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
        else if (Input.GetKey(KeyCode.A)) // 왼쪽으로 갈 때
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
        else // x축 입력 없을 때 0으로
        {
            if (x > 0)
            {
                x -= Time.deltaTime * 20.0f;
            }
            else if (x < 0)
            {
                x += Time.deltaTime * 20.0f;
            }

            if (Mathf.Abs(x) < 0.2f) // 0으로 보정
            {
                x = 0;
            }
        }

        frontHandle.localRotation = Quaternion.Euler(new Vector3(0, x, 0));
        backHandle.localRotation = Quaternion.Euler(new Vector3(0, -x, 0));

        leftWheel.transform.localRotation = Quaternion.Euler(new Vector3(0, x * 30.0f, 0)); // 왼쪽 바퀴 회전
        rightWheel.transform.localRotation = Quaternion.Euler(new Vector3(0, -180.0f + x * 30.0f, 0)); // 오른쪽 바퀴 회전
    }

    private void CarMove()
    {
        Vector3 moveDirection = frontHandle.forward * z;
        Vector3 backDirection = backHandle.forward * z;

        if (z > 0)
        {
            transform.forward = frontHandle.forward;
            rb.MovePosition(rb.position + moveDirection * Time.deltaTime); // 차량 이동
        }
        else if (z < 0)
        {
            transform.forward = backHandle.forward;
            rb.MovePosition(rb.position + backDirection * Time.deltaTime); // 차량 이동
        }
    }
}
