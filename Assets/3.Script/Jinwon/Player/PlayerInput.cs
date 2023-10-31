using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerControl playerControl;
    public bool isRun;
    private bool isAim = false;
    private float z = 0;

    private void Awake()
    {
        TryGetComponent(out playerControl);
    }

    private void Update()
    {
        GetKeyboardInput(); // 키보드 입력 받기
        GetMouseInput();
    }

    private void GetKeyboardInput()
    {
        float x = Input.GetAxis("Horizontal"); // x축 입력 받기

        if (Input.GetKey(KeyCode.LeftShift)) // LeftShift 입력 시 달리기
        {
            isRun = true;
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift)) // 달리기 취소
        {
            isRun = false;
        }

        if (Input.GetKey(KeyCode.W)) // 앞으로 갈 때
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
        else if (Input.GetKey(KeyCode.S)) // 뒤로 갈 때
        {
            if (z > -1)
            {
                z -= Time.deltaTime * 2.0f;
            }

            if (z < -1) z = -1;
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

        playerControl.moveSpeedX = x; // x축 입력값 할당
        playerControl.moveSpeedZ = z; // z축 입력값 할당
    }

    private void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(1) && !isAim) // 우클릭
        {
            isAim = true;
            playerControl.TPSZoomIn();
        }

        if (Input.GetMouseButtonUp(1) && isAim) // 우클릭 해제
        {
            isAim = false;
            playerControl.TPSZoomOut();
        }
    }
}
