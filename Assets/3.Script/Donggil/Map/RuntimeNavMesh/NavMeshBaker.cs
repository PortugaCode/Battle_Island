using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    public GameObject map;

    private Vector3 generatePosition = Vector3.zero;

    private float bakeTime = 100.0f;

    private void Start()
    {
        StartCoroutine(GenerateNavMesh());
    }

    private IEnumerator GenerateNavMesh()
    {
        GameObject obj = Instantiate(map, generatePosition, Quaternion.identity, transform);
        //generatePosition += new Vector3(50, 0, 50);

        yield return new WaitForSeconds(bakeTime);

        NavMeshSurface[] surfaces = gameObject.GetComponentsInChildren<NavMeshSurface>();
        foreach (var s in surfaces)
        {
            s.RemoveData();
            s.BuildNavMesh();
        }


    }

}
