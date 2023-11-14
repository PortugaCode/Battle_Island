using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    [Header("총 프리펩")]
    public GameObject[] GunPrefabs;

    [Header("콜라이더")]
    public GameObject rangeObject;
    private BoxCollider rangeCollider;

    [Header("부모오브젝트 설정")]
    [SerializeField] private Transform Gun_Object;

    [Header("아이템 스폰 개수범위 설정")]
    [Range(0, 10)]
    [SerializeField] private int minItem;
    [Range(0, 20)]
    [SerializeField] private int maxItem;


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

        Vector3 randomPos = new Vector3(rangeX, 1.0f, rangeZ);

        return randomPos;
    }

    private void SpawnGun()
    {
        for (int i = 0; i < Random.Range(minItem, maxItem); i++)
        {
            GameObject gun = Instantiate(GunPrefabs[Random.Range(0, GunPrefabs.Length)], transform.position + RandomPosition(), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 90)));
            gun.transform.SetParent(Gun_Object);
        }
    }
}
