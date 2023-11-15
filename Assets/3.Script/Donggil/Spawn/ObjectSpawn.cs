using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    [Header("오브젝트 프리펩")]
    public GameObject[] MapObjectPrefabs;

    [Header("스폰할 위치")]
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
            Instantiate(MapObjectPrefabs[0], spawnPositions[Random.Range(0, spawnPositions.Length)].transform.position - new Vector3(0, 0.68f, 0), Quaternion.Euler(randomAngle), transform);
        }
        else
        {
            Instantiate(MapObjectPrefabs[0], spawnPositions[Random.Range(0, spawnPositions.Length)].transform.position - new Vector3(0, 0.68f, 0), Quaternion.identity, transform);
        }
    }
}
