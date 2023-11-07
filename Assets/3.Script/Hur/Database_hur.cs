using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database_hur : MonoBehaviour
{
    public List<ItemData_hur> itemList = new List<ItemData_hur>();
    private Sprite _itemIcon;

    private void Start()
    {
        itemList.Add(new ItemData_hur(false, _itemIcon, 100, "아드레날린 주사기", false, ItemData_hur.ItemType.booster, ItemData_hur.UsingType.Consumable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 101, "군용조끼 Lv.1", false, ItemData_hur.ItemType.armor, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 102, "군용조끼 Lv.2", false, ItemData_hur.ItemType.armor, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 103, "군용조끼 Lv.3", false, ItemData_hur.ItemType.armor, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 104, "배낭 Lv.1", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 105, "배낭 Lv.2", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 106, "배낭 Lv.3", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 107, "수류탄", false, ItemData_hur.ItemType.etc, ItemData_hur.UsingType.Consumable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 108, "총알박스", false, ItemData_hur.ItemType.etc, ItemData_hur.UsingType.Consumable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 109, "구급상자", false, ItemData_hur.ItemType.medic, ItemData_hur.UsingType.Consumable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 110, "오토바이 헬멧", false, ItemData_hur.ItemType.head, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 111, "군용헬멧", false, ItemData_hur.ItemType.head, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 112, "용접헬멧", false, ItemData_hur.ItemType.head, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 113, "진통제", false, ItemData_hur.ItemType.booster, ItemData_hur.UsingType.Consumable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 114, "작은 주머니", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 115, "큰 주머니", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 116, "라이플", false, ItemData_hur.ItemType.weapon, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 117, "스나이퍼", false, ItemData_hur.ItemType.weapon, ItemData_hur.UsingType.Wearable));
    }//아이템 데이터베이스

    public ItemData_hur GetItemByID(int itemID) //아이템 아이디를 기반으로 아이템 데이터와 이름을 검색
    {
        foreach (ItemData_hur item in itemList)
        {
            if(item.itemID == itemID)
            {
                return item;
            }
        }

        return null;
    }
}
