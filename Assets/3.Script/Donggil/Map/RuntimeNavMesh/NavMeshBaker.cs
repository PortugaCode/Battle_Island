using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    public NavMeshSurface surface;

    public Transform Map;

    //public GameObject mapPrefab;

    public GameObject randomSpawn;
    public GameObject deadZoneManager;
    public GameObject nextDeadZone;
    public GameObject currentDeadZone;
    public GameObject Player;
    public GameObject SpawnPlane;

    public float bakeTime = 4.0f;
    public float ActiveTime = 2.0f;

    public bool isBakedEnd = false;


    private void Awake()
    {
        StartCoroutine(GenerateNavMesh());
    }

    private IEnumerator GenerateNavMesh()
    {
        //yield return new WaitForSeconds(0.2f);
        //GameObject initMap = Instantiate(mapPrefab, Vector3.zero, Quaternion.identity, transform);

        yield return new WaitForSeconds(bakeTime);
        SetStatic(Map);

        yield return new WaitForSeconds(0.2f);
        BakeWorld();
        isBakedEnd = true;

        if (isBakedEnd)
        {
            yield return new WaitForSeconds(ActiveTime);
            deadZoneManager.SetActive(true);
            nextDeadZone.SetActive(true);
            currentDeadZone.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            //SpawnPlane.SetActive(true);
            randomSpawn.SetActive(true);
            //Player.SetActive(true);
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
