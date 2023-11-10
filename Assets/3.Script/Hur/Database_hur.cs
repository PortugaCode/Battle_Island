using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Database_hur : MonoBehaviour
{
    public static Database_hur instance; // �̱��� ������ ����Ͽ� �ν��Ͻ��� ����

    public List<ItemData_hur> itemList = new List<ItemData_hur>();//�÷��̾ ������ ������ ����Ʈ
    public Image _itemIcon;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // �̹� �ٸ� �ν��Ͻ��� �����ϸ� ���� ������ ���� �ı�
        }
    }

    private void Start()
    //������ �����ͺ��̽�
    {
        Initialize_itemData();
    }

    private void Initialize_itemData()
    {
        itemList.Add(new ItemData_hur(false, 100, "�Ƶ巹���� �ֻ��", false, ItemData_hur.ItemType.booster, ItemData_hur.UsingType.Consumable, 1));
        itemList.Add(new ItemData_hur(false, 101, "�������� Lv.1", false, ItemData_hur.ItemType.armor, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(false, 102, "�������� Lv.2", false, ItemData_hur.ItemType.armor, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(false, 103, "�������� Lv.3", false, ItemData_hur.ItemType.armor, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(false, 104, "�賶 Lv.1", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(false, 105, "�賶 Lv.2", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(false, 106, "�賶 Lv.3", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(false, 107, "����ź", false, ItemData_hur.ItemType.etc, ItemData_hur.UsingType.Consumable, 1));
        itemList.Add(new ItemData_hur(false, 108, "�Ѿ˹ڽ�", false, ItemData_hur.ItemType.etc, ItemData_hur.UsingType.Consumable, 30));
        itemList.Add(new ItemData_hur(false, 109, "���޻���", false, ItemData_hur.ItemType.medic, ItemData_hur.UsingType.Consumable, 1));
        itemList.Add(new ItemData_hur(false, 110, "������� ���", false, ItemData_hur.ItemType.head, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(false, 111, "�������", false, ItemData_hur.ItemType.head, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(false, 112, "�������", false, ItemData_hur.ItemType.head, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(false, 113, "������", false, ItemData_hur.ItemType.booster, ItemData_hur.UsingType.Consumable, 1));
        itemList.Add(new ItemData_hur(false, 114, "���� �ָӴ�", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(false, 115, "ū �ָӴ�", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(false, 116, "������", false, ItemData_hur.ItemType.weapon, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(false, 117, "��������", false, ItemData_hur.ItemType.weapon, ItemData_hur.UsingType.Wearable, 1));
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

    //public void GetItem(int _itemID, int _count) //�����ͺ��̽� ������ �˻�
    //{
    //    for (int i = 0; i < itemList.Count; i++) // �����ͺ��̽��� ������ �߰�
    //    {
    //        if (_itemID == itemList[i].itemID)
    //        {
    //            for (int j = 0; j < itemList.Count; j++) // ����ǰ�� ���� �������� �ִ��� �˻�
    //            {
    //                if(itemList[i].itemID == _itemID) // ����ǰ�� ���� �������� ������ -> ������ ����
    //                {
    //                    itemList[j].itemCount += _count;
    //                    return;
    //                }
    //            }
    //            itemList.Add(itemList[i]); // ����ǰ�� �ش� ������ �߰�
    //            return;
    //        }
    //    }
    //}
}
