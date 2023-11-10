 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    //[SerializeField] GameObject itemPrefab; //�����۸��� �ٸ���


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        //CollectItem();
    //        gameObject.SetActive(false);
    //        Debug.Log($"�������� tag�� : {itemPrefab.tag}");
    //    }
    //}
    [SerializeField] private GameObject itemPrefab; // �������� Inspector���� ��������� �մϴ�.

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ItemPrefab itemPrefabScript = itemPrefab.GetComponent<ItemPrefab>();

            if (itemPrefabScript != null)
            {
                ItemData_hur itemData = itemPrefabScript.ItemData;

                if (itemData != null)
                {
                    Debug.Log($"�������� �̸�: {itemData.itemName}");
                    // ���⿡�� �ٸ� ������ ������ ��� ����
                }
                else
                {
                    Debug.LogWarning("ItemData not found!");
                }
            }
            else
            {
                Debug.LogWarning("ItemPrefab script not found!");
            }

            gameObject.SetActive(false);
        }
    }
}
