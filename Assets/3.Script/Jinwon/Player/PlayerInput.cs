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
        GetInput(); // �Է� �ޱ�
    }

    private void GetInput()
    {
        float x = Input.GetAxis("Horizontal"); // x�� �Է� �ޱ�

        if (Input.GetKey(KeyCode.LeftShift)) // LeftShift �Է� �� �޸���
        {
            isRun = true;
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
        }

        if (Input.GetKey(KeyCode.W)) // ������
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
        else if (Input.GetKey(KeyCode.S)) // �ڷ�
        {
            if (z > -1)
            {
                z -= Time.deltaTime * 2.0f;
            }

            if (z < -1) z = -1;
        }
        else // z�� �Է� ����
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

        playerMove.direction = new Vector3(x, 0, z); // x��, z�� �Է°��� playerMove�� direction�� �Ҵ�
        playerMove.moveSpeedX = x; // x�� �Է°� �Ҵ�
        playerMove.moveSpeedZ = z; // z�� �Է°� �Ҵ�

    }
}