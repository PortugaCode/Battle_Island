using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database_hur : MonoBehaviour
{
    public List<ItemData_hur> itemList = new List<ItemData_hur>();
    
    private void Start()
    {
        itemList.Add(new ItemData_hur(false, 100, "�Ƶ巹���� �ֻ��", false, ItemData_hur.ItemType.booster, ItemData_hur.UsingType.Consumable));
        itemList.Add(new ItemData_hur(false, 101, "1���� ��������", false, ItemData_hur.ItemType.armor, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 102, "2���� ��������", false, ItemData_hur.ItemType.armor, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 103, "3���� �������� ", false, ItemData_hur.ItemType.armor, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 104, "1���� �賶", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 105, "2���� �賶", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 106, "3���� �賶", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 107, "����ź", false, ItemData_hur.ItemType.etc, ItemData_hur.UsingType.Consumable));
        itemList.Add(new ItemData_hur(false, 108, "�Ѿ˹ڽ�", false, ItemData_hur.ItemType.etc, ItemData_hur.UsingType.Consumable));
        itemList.Add(new ItemData_hur(false, 109, "���޻���", false, ItemData_hur.ItemType.medic, ItemData_hur.UsingType.Consumable));
        itemList.Add(new ItemData_hur(false, 110, "������� ���", false, ItemData_hur.ItemType.head, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 111, "�������", false, ItemData_hur.ItemType.head, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 112, "�������", false, ItemData_hur.ItemType.head, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 113, "������", false, ItemData_hur.ItemType.booster, ItemData_hur.UsingType.Consumable));
        itemList.Add(new ItemData_hur(false, 114, "���� �ָӴ�", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 115, "ū �ָӴ�", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 116, "������", false, ItemData_hur.ItemType.weapon, ItemData_hur.UsingType.Wearable));
        itemList.Add(new ItemData_hur(false, 117, "��������", false, ItemData_hur.ItemType.weapon, ItemData_hur.UsingType.Wearable));
    }
}
