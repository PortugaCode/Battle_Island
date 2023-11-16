using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckContainerDockSpawn : MonoBehaviour
{
    [Header("BoxCollider�� Center, Size �Ȱ��� �Է�")]
    public Vector3 colliderPos = Vector3.zero;
    public Vector3 size = Vector3.zero;


    [Header("������Ʈ ������")]
    public GameObject[] MapObjectPrefabs;

    [Header("������ ��ġ")]
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

        colliders = Physics.OverlapBox(objectCenter, size / 2, transform.rotation, layer);      //�� ���̾ Ž��

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
