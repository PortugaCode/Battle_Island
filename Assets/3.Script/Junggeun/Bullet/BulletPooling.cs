using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooling : MonoBehaviour
{
    public static BulletPooling Instance;

    //오브젝트 풀링 인스턴스
    [SerializeField] private GameObject bulletPreb;
    public Queue<GameObject> Bullets = new Queue<GameObject>();
    private int totalbullet = 200;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        for(int i =0; i < totalbullet; i++)
        {
            GameObject a = Instantiate(bulletPreb, transform.position, Quaternion.identity);
            a.transform.SetParent(transform);
            a.gameObject.SetActive(false);
            Bullets.Enqueue(a);
        }
    }






}
