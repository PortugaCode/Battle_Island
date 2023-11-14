using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    public GameObject map;

    private Vector3 generatePosition = new Vector3(50, 0, 50);


    private void Start()
    {
        //GenerateNavMesh();
    }

    private void GenerateNavMesh()
    {
        GameObject obj = Instantiate(map, generatePosition, Quaternion.identity, transform);
        generatePosition += new Vector3(50, 0, 50);


    }

    private void GenerateNavMeshBake()
    {
        NavMeshSurface[] surfaces = gameObject.GetComponentsInChildren<NavMeshSurface>();

        foreach (var s in surfaces)
        {
            s.RemoveData();
            s.BuildNavMesh();
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GenerateNavMesh();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            GenerateNavMeshBake();
        }
    }
}
