using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawn : MonoBehaviour
{
    public GameObject[] mapObjects;

    public List<GameObject> mapObj;

    public Transform spawnPosition;

    public Collider spawnRange;

    public float radius = 5.0f;


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

        randomPos = new Vector3(randomX, -0.68f, randomZ);

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
