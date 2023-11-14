using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawn : MonoBehaviour
{
    [Header("맵에 넣을 오브젝트")]
    public GameObject[] mapObjects;

    [Header("여기는 추가 안해도 됨")]
    public List<GameObject> mapObj;

    [Header("맵(범위)을 넣으면 됨")]
    public Transform spawnPosition;
    public Collider spawnRange;

    //public float radius = 5.0f;

    public bool isRoad = false;

    [Range(0, 500)]
    public int rangeStart = 0;
    [Range(0, 800)]
    public int rangeEnd = 50;


    private void Start()
    {
        transform.position = spawnPosition.position;
        ObjectRandomPosition();
    }

    private Vector3 RandomPosition()
    {
        Vector3 randomPos;
        float rangeX = spawnRange.bounds.size.x;
        float rangeZ = spawnRange.bounds.size.z;

        float randomX = Random.Range((rangeX / 2) * (-1), (rangeX / 2));
        float randomZ = Random.Range((rangeZ / 2) * (-1), (rangeZ / 2));

        if (isRoad)
        {
            randomPos = new Vector3(randomX, 0, randomZ);
        }
        else
        {
            randomPos = new Vector3(randomX, -0.68f, randomZ);
        }

        return randomPos;
    }


    private void ObjectRandomPosition()
    {
        Vector3 randomAngle;
        for (int i = 0; i < Random.Range(rangeStart, rangeEnd); i++)
        {
            randomAngle = new Vector3(0, Random.Range(0, 360), 0);
            mapObj.Add(Instantiate(mapObjects[Random.Range(0, mapObjects.Length)], transform.position + RandomPosition(), Quaternion.Euler(randomAngle), transform));
        }

    }
}
