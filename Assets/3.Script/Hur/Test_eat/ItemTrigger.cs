 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    //[SerializeField] GameObject itemPrefab; //아이템마다 다르게


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        //CollectItem();
    //        gameObject.SetActive(false);
    //        Debug.Log($"아이템의 tag는 : {itemPrefab.tag}");
    //    }
    //}
    [SerializeField] private GameObject itemPrefab; // 프리팹을 Inspector에서 연결해줘야 합니다.

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
                    Debug.Log($"아이템의 이름: {itemData.itemName}");
                    // 여기에서 다른 아이템 데이터 사용 가능
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
