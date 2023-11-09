using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolControl : MonoBehaviour
{
    public static ObjectPoolControl instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        bulletQueue = new Queue<GameObject>();

        SetQueue();
    }

    [Header("Prefab")]
    [SerializeField] private GameObject bulletPrefab;

    [Header("ObjectPool")]
    public int bulletCount = 30;
    public Queue<GameObject> bulletQueue;

    private void SetQueue()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject currentBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            currentBullet.transform.SetParent(transform);
            currentBullet.SetActive(false);
            bulletQueue.Enqueue(currentBullet);
        }
    }
}
