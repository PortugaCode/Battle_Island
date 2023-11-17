using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    public NavMeshSurface surface;

    public Transform Map;

    public GameObject randomSpawn;
    public GameObject deadZoneManager;
    public GameObject nextDeadZone;
    public GameObject currentDeadZone;

    private float bakeTime = 4.0f;

    public bool isBakedEnd = false;

    private void Awake()
    {
        StartCoroutine(GenerateNavMesh());
    }

    private IEnumerator GenerateNavMesh()
    {
        //generatePosition += new Vector3(50, 0, 50);

        yield return new WaitForSeconds(bakeTime);
        SetStatic(Map);

        yield return new WaitForSeconds(0.2f);
        BakeWorld();
        isBakedEnd = true;

        if (isBakedEnd)
        {
            yield return new WaitForSeconds(5.0f);
            deadZoneManager.SetActive(true);
            nextDeadZone.SetActive(true);
            currentDeadZone.SetActive(true);
            randomSpawn.SetActive(true);
        }
    }

    //하위 모든 오브젝트 Static으로 변경
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
    /*
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                BakeWorld();
            }
        }
    */
}
