using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    /*
     * 플레이어가 아이템을 먹으면 enum(itemID)으로 어떤 타입을 먹었는지 정보를 저장
     * 아이템에 직접 코드를 붙이는게 나을듯
     */
    //[SerializeField] private Rigidbody player_r;
    //[SerializeField] private float Speed = 8f;
    ////[SerializeField] private Database_hur database;
    ////public ItemData_C itemData;

    //public bool eatItem = false;
    //public bool getItem = false;

    ////---------------
    //private void Start()
    //{
    //    player_r = GetComponent<Rigidbody>();
    //    //database = FindObjectOfType<Database_hur>();
    //}
    //private void Update()
    //{
    //    float x = Input.GetAxis("Horizontal");
    //    float z = Input.GetAxis("Vertical");
    //    Vector3 value = new Vector3(x, 0f, z) * Speed;
    //    player_r.velocity = value;

    //}

    //private void OnTriggerEnter(Collider other)
    ////아이템에 닿으면 아이템이 터지면서 inventory 목록에 들어감
    //{
    //    if (!eatItem && other.CompareTag("Wearable"))
    //    {
    //        eatItem = true;
    //        getItem = true;
    //        Destroy(other.gameObject);
    //        //Debug.Log("1. 장착가능한 아이템을 얻었다");
    //        //Debug.Log(eatItem);
    //    }
    //    else if (!eatItem && other.CompareTag("Consumer"))
    //    {
    //        eatItem = true;
    //        getItem = true;
    //        Destroy(other.gameObject);
    //        //Debug.Log("1. extra 아이템을 얻었다");
    //        //Debug.Log(eatItem);
    //    }

    //}

}
