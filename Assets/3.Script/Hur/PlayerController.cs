using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    /*
     * �÷��̾ �������� ������ enum(itemID)���� � Ÿ���� �Ծ����� ������ ����
     * �����ۿ� ���� �ڵ带 ���̴°� ������
     */
    [SerializeField] private Rigidbody player_r;
    [SerializeField] private float Speed = 8f;
    [SerializeField] private Database_hur database;
    
    public bool eatItem = false;

    //---------------
    private void Start()
    {
        player_r = GetComponent<Rigidbody>();
        database = FindObjectOfType<Database_hur>();
    }
    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 value = new Vector3(x, 0f, z) * Speed;
        player_r.velocity = value;

    }

    private void OnTriggerEnter(Collider other)
    //�����ۿ� ������ �������� �����鼭 inventory ��Ͽ� ��
    {
        if (!eatItem && other.CompareTag("Wearable"))
        {
            eatItem = true;
            Destroy(other.gameObject);
            Debug.Log(eatItem);
            //Debug.Log("1. ���������� �������� �����");
        }
        else if (!eatItem && other.CompareTag("Consumer"))
        {
            eatItem = true;
            Destroy(other.gameObject);
            Debug.Log(eatItem);
            //Debug.Log("1. extra �������� �����");
        }

    }
}
