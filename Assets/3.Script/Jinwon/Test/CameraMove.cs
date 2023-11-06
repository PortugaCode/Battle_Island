using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private GameObject target; // 추적할 대상
    Vector3 offset; // 차이

    private void Awake()
    {
        offset = transform.position - target.transform.position; // 차이 계산
    }

    private void FixedUpdate()
    {
        transform.position = target.transform.position + offset; // 차이만큼 거리 두고 대상 추적
    }
}
