using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database_hur : MonoBehaviour
{
    public List<ItemData_hur> itemList = new List<ItemData_hur>();
    private Sprite _itemIcon;

    private void Start()
    {
        itemList.Add(new ItemData_hur(false, _itemIcon, 100, "�Ƶ巹���� �ֻ��", false, ItemData_hur.ItemType.booster, ItemData_hur.UsingType.Consumable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 101, "�������� Lv.1", false, ItemData_hur.ItemType.armor, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 102, "�������� Lv.2", false, ItemData_hur.ItemType.armor, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 103, "�������� Lv.3", false, ItemData_hur.ItemType.armor, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 104, "�賶 Lv.1", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 105, "�賶 Lv.2", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 106, "�賶 Lv.3", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 107, "����ź", false, ItemData_hur.ItemType.etc, ItemData_hur.UsingType.Consumable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 108, "�Ѿ˹ڽ�", false, ItemData_hur.ItemType.etc, ItemData_hur.UsingType.Consumable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 109, "���޻���", false, ItemData_hur.ItemType.medic, ItemData_hur.UsingType.Consumable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 110, "������� ���", false, ItemData_hur.ItemType.head, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 111, "�������", false, ItemData_hur.ItemType.head, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 112, "�������", false, ItemData_hur.ItemType.head, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 113, "������", false, ItemData_hur.ItemType.booster, ItemData_hur.UsingType.Consumable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 114, "���� �ָӴ�", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 115, "ū �ָӴ�", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 116, "������", false, ItemData_hur.ItemType.weapon, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, _itemIcon, 117, "��������", false, ItemData_hur.ItemType.weapon, ItemData_hur.UsingType.Wearable));
    }//������ �����ͺ��̽�

    public ItemData_hur GetItemByID(int itemID) //������ ���̵� ������� ������ �����Ϳ� �̸��� �˻�
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
