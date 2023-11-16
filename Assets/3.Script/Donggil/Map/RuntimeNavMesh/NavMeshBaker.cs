using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    public GameObject map;

    private Vector3 generatePosition = Vector3.zero;

    private float bakeTime = 4.0f;
    private float gameTime = 0;

    private void Awake()
    {
        StartCoroutine(GenerateNavMesh());
    }

    private IEnumerator GenerateNavMesh()
    {
        GameObject obj = Instantiate(map, generatePosition, Quaternion.identity, transform);
        //generatePosition += new Vector3(50, 0, 50);

        yield return new WaitForSeconds(bakeTime);

        SetStatic(transform);
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
        NavMeshSurface[] surfaces = gameObject.GetComponentsInChildren<NavMeshSurface>();
        foreach (var s in surfaces)
        {
            s.RemoveData();
            s.BuildNavMesh();
        }

    }
/*
    private void Update()
    {
        gameTime += Time.deltaTime;
        if (gameTime > 1.0f)
        {
            BakeWorld();
            gameTime = 0;
        }
    }
*/
}
