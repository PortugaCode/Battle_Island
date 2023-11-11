 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class ItemTrigger : MonoBehaviour
{
    public ItemData_hur itemData; // 인스턴스를 Inspector에서 할당
    public ItemData_C itemData_c; //Diablo 인벤토리에도 정보를 줘야 함
    //[SerializeField] List<ItemData_C> itemData_C; //Diablo 인벤토리에도 정보를 줘야 함

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (itemData != null)
            {
                //Debug.Log($"이름: {itemData.itemName}, 아이디: {itemData.itemID}, 개수: {itemData.itemCount}");

                PlayerPrefs.SetInt("아이템 ID", itemData.itemID);
                PlayerPrefs.SetString("아이템 이름", itemData.itemName);
                PlayerPrefs.SetInt("아이템 개수", itemData.itemCount);

                //Diablo 인벤토리에도 정보를 줘야 함
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
    public void RememberInfo()
    {
        
    }
}
