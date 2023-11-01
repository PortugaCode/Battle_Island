using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private Vector3 offset;

    public float turnSpeed = 4.0f; // 마우스 회전 속도

    private float xRotate = 0.0f; // 내부 사용할 X축 회전량은 별도 정의

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
        // 좌우 회전
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;

        // 현재 y축 회전값에 더한 새로운 회전각도 계산
        float yRotate = transform.eulerAngles.y + yRotateSize;

        // 상하 회전
        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;

        // 각도 제한
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);

        // 회전
        //transform.eulerAngles = new Vector3(xRotate, yRotate, 0);

        player.transform.eulerAngles += new Vector3(0, yRotate, 0);
    }
}
