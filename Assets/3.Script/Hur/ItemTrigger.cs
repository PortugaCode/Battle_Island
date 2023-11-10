 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class ItemTrigger : MonoBehaviour
{
    public ItemData_hur itemData; // 인스턴스를 Inspector에서 할당
    [SerializeField] private List<GameObject> playerSeeItem = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (itemData != null)
            {
                Debug.Log($"이름: {itemData.itemName}, 아이디: {itemData.itemID}, 개수: {itemData.itemCount}");

                PlayerPrefs.SetInt("아이템 ID", itemData.itemID);
                PlayerPrefs.SetString("아이템 이름", itemData.itemName);
                PlayerPrefs.SetInt("아이템 개수", itemData.itemCount++);
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
