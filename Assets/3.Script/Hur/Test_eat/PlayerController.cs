using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody player_r;
    [SerializeField] private float Speed = 8f;
    public bool eatItem = false;

    private void Start()
    {
        player_r = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 value = new Vector3(x, 0f, z) * Speed;
        player_r.velocity = value;
    }
    private void OnTriggerEnter(Collider other)
        //아이템에 닿으면 아이템이 터지면서 inventory 목록에 들어감
    {
        if (!eatItem && other.CompareTag("Wearable"))
        {
            eatItem = true;
            Destroy(other.gameObject);
            //Debug.Log("장착가능한 아이템을 얻었다");
        }
        else if (!eatItem && other.CompareTag("Consumer"))
        {
            eatItem = true;
            Destroy(other.gameObject);
            //Debug.Log("extra 아이템을 얻었다");
        }
    }

}
