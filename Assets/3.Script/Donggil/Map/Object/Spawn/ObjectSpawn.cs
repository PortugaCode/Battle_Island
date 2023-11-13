using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    [Header("������Ʈ ������")]
    public GameObject[] MapObjectPrefabs;

    [Header("������ ��ġ")]
    public GameObject[] spawnPositions;


    private CheckObject check;
    private Vector3 randomAngle;
    [SerializeField] private bool isRotated = false;

    private void Awake()
    {
        check = FindObjectOfType<CheckObject>();
    }


    private void Start()
    {
        SpawnObject();
    }
    private void SpawnObject()
    {
        randomAngle = new Vector3(0, Random.Range(0, 360), 0);
        if (isRotated)
        {
            for (int i = 0; i < spawnPositions.Length; i++)
            {
                Instantiate(MapObjectPrefabs[0], spawnPositions[i].transform.position - new Vector3(0, 0.68f, 0), Quaternion.Euler(randomAngle), transform);
            }
        }
        else
        {
            for (int i = 0; i < spawnPositions.Length; i++)
            {
                Instantiate(MapObjectPrefabs[0], spawnPositions[i].transform.position - new Vector3(0, 0.68f, 0), Quaternion.identity, transform);
            }
        }
    }
}
