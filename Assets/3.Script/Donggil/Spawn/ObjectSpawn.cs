using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    [Header("오브젝트 프리펩")]
    public GameObject[] MapObjectPrefabs;

    [Header("콜라이더")]
    public GameObject rangeObject;
    private BoxCollider rangeCollider;

    [SerializeField] private Transform Map_Object;

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


        Vector3 randomPos = new Vector3(rangeX, transform.position.y, rangeZ) + originPos;
        

        return randomPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Wall"))
        {
            RandomPosition();
        }
    }

    private void SpawnGun()
    {
        for (int i = 0; i < Random.Range(20, 42); i++)
        {
            GameObject mapObject = Instantiate(MapObjectPrefabs[Random.Range(0, MapObjectPrefabs.Length)], RandomPosition(), Quaternion.identity);
            mapObject.transform.SetParent(Map_Object);
        }
    }
}
