using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Database_hur : MonoBehaviour
{
    public List<ItemData_hur> itemList = new List<ItemData_hur>();
    public Image _itemIcon;

    private void Start()
    //아이템 데이터베이스
    {
        itemList.Add(new ItemData_hur(false, 100, "아드레날린 주사기", false, ItemData_hur.ItemType.booster, ItemData_hur.UsingType.Consumable));
        itemList.Add(new ItemData_hur(false, 101, "군용조끼 Lv.1", false, ItemData_hur.ItemType.armor, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 102, "군용조끼 Lv.2", false, ItemData_hur.ItemType.armor, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 103, "군용조끼 Lv.3", false, ItemData_hur.ItemType.armor, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 104, "배낭 Lv.1", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 105, "배낭 Lv.2", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 106, "배낭 Lv.3", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 107, "수류탄", false, ItemData_hur.ItemType.etc, ItemData_hur.UsingType.Consumable));
        itemList.Add(new ItemData_hur(false, 108, "총알박스", false, ItemData_hur.ItemType.etc, ItemData_hur.UsingType.Consumable));
        itemList.Add(new ItemData_hur(false, 109, "구급상자", false, ItemData_hur.ItemType.medic, ItemData_hur.UsingType.Consumable));
        itemList.Add(new ItemData_hur(false, 110, "오토바이 헬멧", false, ItemData_hur.ItemType.head, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 111, "군용헬멧", false, ItemData_hur.ItemType.head, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 112, "용접헬멧", false, ItemData_hur.ItemType.head, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 113, "진통제", false, ItemData_hur.ItemType.booster, ItemData_hur.UsingType.Consumable));
        itemList.Add(new ItemData_hur(false, 114, "작은 주머니", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 115, "큰 주머니", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 116, "라이플", false, ItemData_hur.ItemType.weapon, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 117, "스나이퍼", false, ItemData_hur.ItemType.weapon, ItemData_hur.UsingType.Wearable));
    }

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
