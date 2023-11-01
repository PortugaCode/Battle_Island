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
        GetKeyboardInput(); // Ű���� �Է� �ޱ�
        GetMouseInput();
    }

    private void GetKeyboardInput()
    {
        float x = Input.GetAxis("Horizontal"); // x�� �Է� �ޱ�

        if (Input.GetKey(KeyCode.LeftShift)) // LeftShift �Է� �� �޸���
        {
            isRun = true;
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift)) // �޸��� ���
        {
            isRun = false;
        }

        if (Input.GetKey(KeyCode.W)) // ������ �� ��
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
        else if (Input.GetKey(KeyCode.S)) // �ڷ� �� ��
        {
            if (z > -1)
            {
                z -= Time.deltaTime * 2.0f;
            }

            if (z < -1) z = -1;
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

        playerControl.moveSpeedX = x; // x�� �Է°� �Ҵ�
        playerControl.moveSpeedZ = z; // z�� �Է°� �Ҵ�
    }

    private void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(1) && !isAim) // ��Ŭ��
        {
            isAim = true;
            playerControl.TPSZoomIn();
        }

        if (Input.GetMouseButtonUp(1) && isAim) // ��Ŭ�� ����
        {
            isAim = false;
            playerControl.TPSZoomOut();
        }
    }
}
