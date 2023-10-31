using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMove playerMove;
    private bool isRun;
    private float z = 0;

    private void Awake()
    {
        TryGetComponent(out playerMove);
    }

    private void Update()
    {
        GetInput(); // 입력 받기
    }

    private void GetInput()
    {
        float x = Input.GetAxis("Horizontal"); // x축 입력 받기

        if (Input.GetKey(KeyCode.LeftShift)) // LeftShift 입력 시 달리기
        {
            isRun = true;
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
        }

        if (Input.GetKey(KeyCode.W)) // 앞으로
        {
            if (isRun == true)
            {
                if (z < 2)
                {
                    z += Time.deltaTime * 2.0f;
                }

                if (z > 2) z = 2;
            }
            else if (isRun == false)
            {
                if (z < 1)
                {
                    z += Time.deltaTime * 2.0f;
                }

                if (z > 1) z -= Time.deltaTime * 2.0f;
            }
        }
        else if (Input.GetKey(KeyCode.S)) // 뒤로
        {
            if (z > -1)
            {
                z -= Time.deltaTime * 2.0f;
            }

            if (z < -1) z = -1;
        }
        else // z축 입력 없음
        {
            if (z > 0)
            {
                z -= Time.deltaTime * 10.0f;
            }
            else if (z < 0)
            {
                z += Time.deltaTime * 10.0f;
            }

            if (Mathf.Abs(z) <= 0.001f)
            {
                z = 0;
            }
        }

        playerMove.direction = new Vector3(x, 0, z); // x축, z축 입력값을 playerMove의 direction에 할당
        playerMove.moveSpeedX = x; // x축 입력값 할당
        playerMove.moveSpeedZ = z; // z축 입력값 할당

    }
}
