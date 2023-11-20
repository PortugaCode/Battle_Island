using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Database_hur : MonoBehaviour
{
    public List<ItemData_hur> itemList = new List<ItemData_hur>();//�÷��̾ ������ ������ ����Ʈ

    private void Start()
    //������ �����ͺ��̽� �ʱ�ȭ
    {
        Initialize_itemData();
    }

    public void Initialize_itemData()
    {
        itemList.Add(new ItemData_hur(100, "�Ƶ巹���� �ֻ��", false, ItemData_hur.ItemType.booster, ItemData_hur.UsingType.Consumable, 1, 3, 1));
        itemList.Add(new ItemData_hur(101, "�������� Lv.1", false, ItemData_hur.ItemType.armor, ItemData_hur.UsingType.Wearable, 1, 3, 4));
        itemList.Add(new ItemData_hur(102, "�������� Lv.2", false, ItemData_hur.ItemType.armor, ItemData_hur.UsingType.Wearable, 1, 3, 4));
        itemList.Add(new ItemData_hur(103, "�������� Lv.3", false, ItemData_hur.ItemType.armor, ItemData_hur.UsingType.Wearable, 1, 4, 4));
        itemList.Add(new ItemData_hur(104, "�賶 Lv.1", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable, 1, 3, 4));
        itemList.Add(new ItemData_hur(105, "�賶 Lv.2", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable, 1, 3, 4));
        itemList.Add(new ItemData_hur(106, "�賶 Lv.3", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable, 1, 4, 4));
        itemList.Add(new ItemData_hur(107, "����ź", false, ItemData_hur.ItemType.etc, ItemData_hur.UsingType.Consumable, 1, 2, 2));
        itemList.Add(new ItemData_hur(108, "�Ѿ˹ڽ�", false, ItemData_hur.ItemType.etc, ItemData_hur.UsingType.Consumable, 30, 3, 2));
        itemList.Add(new ItemData_hur(109, "���޻���", false, ItemData_hur.ItemType.medic, ItemData_hur.UsingType.Consumable, 1, 3, 3));
        itemList.Add(new ItemData_hur(110, "������� ���", false, ItemData_hur.ItemType.head, ItemData_hur.UsingType.Wearable, 1, 3, 3));
        itemList.Add(new ItemData_hur(111, "�������", false, ItemData_hur.ItemType.head, ItemData_hur.UsingType.Wearable, 1, 3, 3));
        itemList.Add(new ItemData_hur(112, "�������", false, ItemData_hur.ItemType.head, ItemData_hur.UsingType.Wearable, 1, 3, 3));
        itemList.Add(new ItemData_hur(113, "������", false, ItemData_hur.ItemType.booster, ItemData_hur.UsingType.Consumable, 1, 1, 2));
        itemList.Add(new ItemData_hur(114, "���� �ָӴ�", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable, 1, 2, 2));
        itemList.Add(new ItemData_hur(115, "ū �ָӴ�", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable, 1, 2, 2));
        itemList.Add(new ItemData_hur(116, "������", false, ItemData_hur.ItemType.weapon, ItemData_hur.UsingType.Wearable, 1, 9, 3));
        itemList.Add(new ItemData_hur(117, "��������", false, ItemData_hur.ItemType.weapon, ItemData_hur.UsingType.Wearable, 1, 9, 3));
    }

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

    public int ReturnItemIndex(int itemID)
    {
        for(int i =0; i< itemList.Count; i++)
        {
            if (itemID == itemList[i].itemID)
            {
                return i;
            }
        }
        return -1;
    }
}
