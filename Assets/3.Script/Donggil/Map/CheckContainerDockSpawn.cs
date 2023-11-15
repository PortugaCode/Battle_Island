using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckContainerDockSpawn : MonoBehaviour
{
    [Header("BoxCollider의 Center, Size 똑같이 입력")]
    public Vector3 colliderPos = Vector3.zero;
    public Vector3 size = Vector3.zero;


    [Header("오브젝트 프리펩")]
    public GameObject[] MapObjectPrefabs;

    [Header("스폰할 위치")]
    public GameObject[] spawnPositions;

    public Collider bound;

    public LayerMask layer;


    private void Start()
    {
        SpawnObject();
    }
    private void SpawnObject()
    {
        Collider[] colliders;
        Vector3 objectCenter = colliderPos + transform.position;

        colliderPos = bound.bounds.center;
        size = bound.bounds.size;

        colliders = Physics.OverlapBox(objectCenter, size / 2, transform.rotation, layer);      //이 레이어만 탐지

        foreach(Collider col in colliders)
        {
            if(col.CompareTag("Dongil01") || col.CompareTag("Dongil02"))
            {
                Instantiate(MapObjectPrefabs[0], spawnPositions[0].transform.position - new Vector3(0, 0.68f, 0), Quaternion.identity, transform);
            }
            else
            {
                Instantiate(MapObjectPrefabs[0], spawnPositions[1].transform.position - new Vector3(0, 0.68f, 0), Quaternion.identity, transform);
            }
        }


    }
}
