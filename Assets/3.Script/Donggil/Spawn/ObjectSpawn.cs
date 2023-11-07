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


    private bool isMoveCollider = false;

    Ray ray = new Ray();
    RaycastHit hit;

    private void Awake()
    {
        TryGetComponent(out rangeCollider);
    }

    private void Start()
    {
        //SpawnObject();
        isMoveCollider = true;
    }

    private void Update()
    {
        if (isMoveCollider)
        {
            MoveSpawnObject();
            //isMoveCollider = false;
        }

    }

    private Vector3 RandomPosition()
    {
        Vector3 originPos = rangeCollider.transform.position;

        float rangeX = rangeCollider.bounds.size.x;
        float rangeZ = rangeCollider.bounds.size.z;

        rangeX = Random.Range((rangeX / 2) * -1, (rangeX / 2));
        rangeZ = Random.Range((rangeZ / 2) * -1, (rangeZ / 2));


        Vector3 randomPos = new Vector3(rangeX, 0, rangeZ) + originPos;


        return randomPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            RandomPosition();
        }
    }

    private void SpawnObject()
    {
        for (int i = 0; i < Random.Range(50, 100); i++)
        {
            GameObject mapObject = Instantiate(MapObjectPrefabs[Random.Range(0, MapObjectPrefabs.Length)], RandomPosition(), Quaternion.identity);
            mapObject.transform.SetParent(Map_Object);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            isMoveCollider = false;

        }
    }


    private void MoveSpawnObject()
    {
        ray.origin = transform.position;
        ray.direction = Vector3.right;
        Debug.DrawRay(transform.position, Vector3.down * 15.0f, Color.red);
        if (Physics.Raycast(ray, out hit, 15.0f, (-1) - (1 << LayerMask.NameToLayer("Donggil") ) ) ) 
        {
            transform.Translate(Vector3.right * Time.deltaTime * 20);
        }
    }
}
