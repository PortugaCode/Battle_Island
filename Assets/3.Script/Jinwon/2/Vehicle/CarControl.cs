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

    private float x; // 좌우 (각도)
    private float z; // 앞뒤 (속력)

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

            if (Mathf.Abs(z) <= 0.001f) // 0으로 보정
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
                x -= Time.deltaTime * 30.0f;
            }

            if (x < -xLimit) x = -xLimit;
        }
        else // x축 입력 없을 때 0으로
        {
            if (x > 0)
            {
                x -= Time.deltaTime * 30.0f;
            }
            else if (x < 0)
            {
                x += Time.deltaTime * 30.0f;
            }

            if (Mathf.Abs(x) <= 0.01f) // 0으로 보정
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

            rb.MovePosition(rb.position + moveDirection * Time.deltaTime); // 차량 이동
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 moveDirection = handle.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, moveDirection);
    }
}
