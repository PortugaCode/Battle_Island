using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotData : MonoBehaviour
{
    [SerializeField] public List<GameObject> SurItemSlot = new List<GameObject>();
    public bool touch = false;
    
    //�԰� ����Ʈ�� �ִ� ����
    //�ֺ� ����Ʈ ����� ������ ���� �ְ� Ŭ������ �� ���° �������� Ȯ�� 
    
    private void Update()
    {
        AddSlot();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            touch = true;
        }
    }
    private void AddSlot()
    {
        if (touch)//Player tag�� �浹���� ��
        {
            for (int i = 0; i < SurItemSlot.Count; i++)
            {
                Debug.Log("���� �߰�");
            }
        }
        
    }

    
}
