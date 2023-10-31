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
        GetInput(); // �Է� �ޱ�
    }

    private void GetInput()
    {
        float x = Input.GetAxis("Horizontal"); // x�� �Է� �ޱ�
        float z = Input.GetAxis("Vertical"); // z�� �Է� �ޱ�

        playerMove.direction = new Vector3(x, 0, z); // x��, z�� �Է°��� playerMove�� direction�� �Ҵ�
        playerMove.moveSpeedX = x; // x�� �Է°� �Ҵ�
        

        if (Input.GetKey(KeyCode.LeftShift)) // LeftShift �Է� �� �޸���
        {
            isRun = true;
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
        }

        if (isRun)
        {
            playerMove.moveSpeedZ = z * 2; // z�� �Է°� �Ҵ�
        }
        else
        {
            playerMove.moveSpeedZ = z; // z�� �Է°� �Ҵ�
        }
        
    }
}
