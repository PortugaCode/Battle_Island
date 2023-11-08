using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    [Header("오브젝트 프리펩")]
    public GameObject[] MapObjectPrefabs;
    public GameObject[] MountainObjectPrefabs;

    [Header("콜라이더")]
    public GameObject rangeObject;
    public Collider MapCollider;
    private BoxCollider rangeCollider;

    [SerializeField] private Transform Map_Object;

    [Range(0, 100)]
    public int rangeStart = 0;
    [Range(0, 500)]
    public int rangeEnd = 50;


    private bool isMoveCollider = false;

    Ray ray = new Ray();
    RaycastHit hit;

    private void Awake()
    {
        TryGetComponent(out rangeCollider);
    }

    private void Start()
    {
        SpawnObject();
    }

    private Vector3 RandomPosition()
    {
        Vector3 originPos = rangeCollider.transform.position;

        float rangeX = rangeCollider.bounds.size.x;
        float rangeZ = rangeCollider.bounds.size.z;

        rangeX = Random.Range((rangeX / 2) * -1, (rangeX / 2));
        rangeZ = Random.Range((rangeZ / 2) * -1, (rangeZ / 2));


        Vector3 randomPos = new Vector3(rangeX, -0.8f, rangeZ) + originPos;


        return randomPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void SpawnObject()
    {
        for (int i = 0; i < Random.Range(rangeStart, rangeEnd); i++)
        {
            GameObject mapObject = Instantiate(MapObjectPrefabs[Random.Range(0, MapObjectPrefabs.Length)], RandomPosition() + new Vector3(20, 0, 20), Quaternion.identity);
            mapObject.transform.SetParent(Map_Object);
        }
    }

    private void MountainSpawn()
    {
        float x = MapCollider.bounds.extents.x;
        float z = MapCollider.bounds.extents.x;

        for (int i = 0; i < Random.Range(1, 3); i++)
        {
            GameObject mapObject = Instantiate(MountainObjectPrefabs[Random.Range(0, MountainObjectPrefabs.Length)], RandomPosition() + new Vector3(20, 0, 20), Quaternion.identity);
        }
    }
}
