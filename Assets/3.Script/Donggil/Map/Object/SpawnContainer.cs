using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnContainer : MonoBehaviour
{
    [SerializeField] private GameObject[] containerBoxs;

    private int column = 4;
    private int row = 3;
    private bool isSpawn;

    private void Start()
    {
        SpawnContainerBox();
    }

    private void SpawnContainerBox()
    {
        Vector3 spawnPostion = transform.position;
        Vector3 randomAngle = new Vector3(0, Random.Range(-5.0f, 5.0f), 0);
        Vector3 interval_X = new Vector3(7, 0, 0);
        Vector3 interval_Z = new Vector3(0, 0, -9);
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                isSpawn = (Random.value > 0.2f);
                if (isSpawn)
                {
                    Instantiate(containerBoxs[Random.Range(0, containerBoxs.Length)], spawnPostion + interval_X * j + interval_Z * i, Quaternion.Euler(randomAngle), transform);
                }
            }
        }
    }
}
