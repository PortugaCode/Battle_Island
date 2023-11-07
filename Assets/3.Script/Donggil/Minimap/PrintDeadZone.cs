using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrintDeadZone : MonoBehaviour
{
    LineRenderer line;
    MeshRenderer mesh;
    public float radius;
    public int circlePoint = 100;
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
