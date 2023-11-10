 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class ItemTrigger : MonoBehaviour
{
    public ItemData_hur itemData; // �ν��Ͻ��� Inspector���� �Ҵ�
    [SerializeField] private List<GameObject> playerSeeItem = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (itemData != null)
            {
                Debug.Log($"�̸�: {itemData.itemName}, ���̵�: {itemData.itemID}, ����: {itemData.itemCount}");

                PlayerPrefs.SetInt("������ ID", itemData.itemID);
                PlayerPrefs.SetString("������ �̸�", itemData.itemName);
                PlayerPrefs.SetInt("������ ����", itemData.itemCount++);
            }
            else
            {
                Debug.LogWarning("ItemData not assigned!");
            }

           // gameObject.SetActive(false);
        }
    }
    public void RememberInfo()
    {
        
    }
}
