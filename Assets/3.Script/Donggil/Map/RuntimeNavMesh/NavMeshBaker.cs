using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    public NavMeshSurface surface;

    public Transform Map;

    private float bakeTime = 4.0f;

    private void Awake()
    {
        StartCoroutine(GenerateNavMesh());
    }

    private IEnumerator GenerateNavMesh()
    {
        //generatePosition += new Vector3(50, 0, 50);

        yield return new WaitForSeconds(bakeTime);

        SetStatic(Map);
        yield return new WaitForSeconds(0.5f);

        BakeWorld();
    }

    private void SetStatic(Transform parent)
    {
        parent.gameObject.isStatic = true;

        foreach (Transform child in parent)
        {
            SetStatic(child);
        }
    }

    private void BakeWorld()
    {
        surface.RemoveData();
        surface.BuildNavMesh();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            BakeWorld();
        }
    }

}
