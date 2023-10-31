using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private Vector3 offset;

    public float turnSpeed = 4.0f; // ���콺 ȸ�� �ӵ�

    private float xRotate = 0.0f; // ���� ����� X�� ȸ������ ���� ����

    private void Awake()
    {
        offset = transform.position - player.transform.position;
    }

    private void Update()
    {
        FollowPlayer();
        MouseRotation();
    }

    void FollowPlayer()
    {
        transform.position = player.transform.position + offset;
    }

    void MouseRotation()
    {
        // �¿� ȸ��
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;

        // ���� y�� ȸ������ ���� ���ο� ȸ������ ���
        float yRotate = transform.eulerAngles.y + yRotateSize;

        // ���� ȸ��
        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;

        // ���� ����
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);

        // ȸ��
        //transform.eulerAngles = new Vector3(xRotate, yRotate, 0);

        player.transform.eulerAngles += new Vector3(0, yRotate, 0);
    }
}
