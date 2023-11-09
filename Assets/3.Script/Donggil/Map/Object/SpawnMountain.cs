using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMountain : MonoBehaviour
{
    public float radius = 5.0f;

    public GameObject[] mountain_Prefabs;

    private GameObject mountain;
    public Transform mountainPos;

    public Collider range;

    public int mountainCount = 1;

    private void Start()
    {
        transform.position = mountainPos.position;
        MountainRandomSpawn();
    }

    private void MountainRandomSpawn()
    {
        int randomAngle;

        for (int i = 0; i < mountainCount; i++)
        {
            randomAngle = Random.Range(120, 240);

            //float x = Mathf.Cos(randomAngle * Mathf.Deg2Rad) * radius;
            //float z = Mathf.Sin(randomAngle * Mathf.Deg2Rad) * radius;
            //Vector3 spawnPosition = new Vector3(x, -0.8f, z);

            float x = range.bounds.size.x;
            float z = range.bounds.size.z;

            

            Vector3 spawnPosition = new Vector3(Random.Range((x / 2) * (-1), (x / 2)), -0.8f, Random.Range((z / 2) * (-1), (z / 2)));

            mountain = Instantiate(mountain_Prefabs[Random.Range(0, mountain_Prefabs.Length)], transform.position + spawnPosition, Quaternion.Euler(new Vector3(0, randomAngle, 0)), transform);
            mountain.transform.localScale += new Vector3(0, Random.Range(1.0f, 4.0f), 0);
        }

    }

}
