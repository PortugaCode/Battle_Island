using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurroundInventory : MonoBehaviour
{
    private NoticeItem[] slots;
    private List<ItemData_hur> inventoryItemList; //주변에 감지된 아이템 리스트
    private int selectedItem_s; //선택된 아이템

    public void ShowItem()
    {
        //아이템 감지가 되면 동작
        //itemtype이 명확해야 함
    }
}
