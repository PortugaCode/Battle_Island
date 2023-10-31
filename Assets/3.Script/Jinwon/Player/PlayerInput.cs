using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMove playerMove;
    private bool isRun;

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
        float z = Input.GetAxis("Vertical"); // z축 입력 받기

        playerMove.direction = new Vector3(x, 0, z); // x축, z축 입력값을 playerMove의 direction에 할당
        playerMove.moveSpeedX = x; // x축 입력값 할당
        

        if (Input.GetKey(KeyCode.LeftShift)) // LeftShift 입력 시 달리기
        {
            isRun = true;
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
        }

        if (isRun)
        {
            playerMove.moveSpeedZ = z * 2; // z축 입력값 할당
        }
        else
        {
            playerMove.moveSpeedZ = z; // z축 입력값 할당
        }
        
    }
}
