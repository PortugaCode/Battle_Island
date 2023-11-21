using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PrintDeadZone : MonoBehaviour
{
    LineRenderer line;
    MeshRenderer mesh;
    [Header("MainCamera에 Culling Mask DeadZone 레이어 해제")]
    [Tooltip("MainCamera에 Rendering에서 Culling Mask \nDeadZone레이어 해제")]
    [SerializeField] private int circlePoint = 100;      //점 개수 100개로 원과 유사하게 만듦
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