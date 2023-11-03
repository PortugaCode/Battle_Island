using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    [Header("ÃÑ ÇÁ¸®Æé")]
    public GameObject[] GunPrefabs;

    [Header("ÄÝ¶óÀÌ´õ")]
    public GameObject rangeObject;
    private BoxCollider rangeCollider;

    [SerializeField] private Transform Gun_Object;

    private void Awake()
    {
        TryGetComponent(out rangeCollider);
    }

    private void Start()
    {
        SpawnGun();
    }

    private Vector3 RandomPosition()
    {
        Vector3 originPos = rangeCollider.transform.position;

        float rangeX = rangeCollider.bounds.size.x;
        float rangeZ = rangeCollider.bounds.size.z;

        rangeX = Random.Range((rangeX / 2) * -1, (rangeX / 2));
        rangeZ = Random.Range((rangeZ / 2) * -1, (rangeZ / 2));

        Vector3 randomPos = new Vector3(rangeX, transform.position.y, rangeZ);

        return randomPos;
    }

    private void SpawnGun()
    {
        for (int i = 0; i < Random.Range(5, 11); i++)
        {
            GameObject gun = Instantiate(GunPrefabs[Random.Range(0, GunPrefabs.Length)], RandomPosition(), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 90)));
            gun.transform.SetParent(Gun_Object);
        }
    }
}
