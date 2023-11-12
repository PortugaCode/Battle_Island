 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    public ItemData_hur itemData; // 인스턴스를 Inspector에서 할당
    public ItemData_C itemData_c; //Diablo 인벤토리에도 정보를 줘야 함

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (itemData != null)
            {
                //최근에 저장한 정보 하나만 뜸
                PlayerPrefs.SetInt("아이템 ID", itemData.itemID);
                PlayerPrefs.SetString("아이템 이름", itemData.itemName);
                PlayerPrefs.SetInt("아이템 개수", itemData.itemCount);

                //Diablo 인벤토리에도 정보를 줘야 함
                //최근에 저장한 정보 하나만 뜸
                PlayerPrefs.SetInt("Item_ID", itemData_c.itemID);
                Debug.Log($"Diblo에게 전할 것 : {PlayerPrefs. GetInt("Item_ID")}");

                Debug.Log($"2. 이것은 {PlayerPrefs.GetString("아이템 이름")}이다.");
            }
            else
            {
                Debug.LogWarning("아이템 데이터를 찾을 수 없습니다!");
            }
        }
    }
}
