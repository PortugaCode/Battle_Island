using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrintDeadZone : MonoBehaviour
{
    LineRenderer line;
    MeshRenderer mesh;

    [Header("MainCamera 설정방법(마우스 올려 툴팁확인)")]
    [Tooltip("MainCamera에 Rendering부분에서 Culling Mask \nDeadZone 체크 해제하기")]
    [SerializeField] private int circlePoint = 100;      //점 개수 100개로 해서 유사 원으로 만듦
    private float radius;

    private DeadZone DZone;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        mesh = GetComponentInChildren<MeshRenderer>();
        DZone = FindObjectOfType<DeadZone>();

        line.loop = true;
    }

    private void Update()
    {
        CreatPoint();
    }

    private void CreatPoint()
    {
        
        radius = mesh.bounds.size.x / 2;
        line.positionCount = circlePoint;

        float anglePerPoint = 2 * Mathf.PI * ((float)1 / circlePoint);

        for (int i = 0; i < circlePoint; i++)
        {
            Vector3 point = transform.position;
            float angle = anglePerPoint * i;

            point.x += Mathf.Cos(angle) * radius;
            point.y = 1f;
            point.z += Mathf.Sin(angle) * radius;

            line.SetPosition(i, point);
        }
    }
}
