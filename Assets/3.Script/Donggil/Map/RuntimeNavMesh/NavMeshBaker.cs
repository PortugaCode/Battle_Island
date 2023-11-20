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
    public GameObject Player;

    public float bakeTime = 4.0f;
    public float ActiveTime = 2.0f;

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
            yield return new WaitForSeconds(ActiveTime);
            deadZoneManager.SetActive(true);
            nextDeadZone.SetActive(true);
            currentDeadZone.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            randomSpawn.SetActive(true);
            //Player.SetActive(true);
        }
    }

    //���� ��� ������Ʈ Static���� ����
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
