using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Database_hur : MonoBehaviour
{
    public static Database_hur instance;

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
            Destroy(gameObject);
        }
    }

    private void Start()
    //������ �����ͺ��̽� �ʱ�ȭ
    {
        Initialize_itemData();
    }

    public void Initialize_itemData()
    {
        itemList.Add(new ItemData_hur(100, "�Ƶ巹���� �ֻ��", false, ItemData_hur.ItemType.booster, ItemData_hur.UsingType.Consumable, 1));
        itemList.Add(new ItemData_hur(101, "�������� Lv.1", false, ItemData_hur.ItemType.armor, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(102, "�������� Lv.2", false, ItemData_hur.ItemType.armor, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(103, "�������� Lv.3", false, ItemData_hur.ItemType.armor, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(104, "�賶 Lv.1", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(105, "�賶 Lv.2", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(106, "�賶 Lv.3", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(107, "����ź", false, ItemData_hur.ItemType.etc, ItemData_hur.UsingType.Consumable, 1));
        itemList.Add(new ItemData_hur(108, "�Ѿ˹ڽ�", false, ItemData_hur.ItemType.etc, ItemData_hur.UsingType.Consumable, 30));
        itemList.Add(new ItemData_hur(109, "���޻���", false, ItemData_hur.ItemType.medic, ItemData_hur.UsingType.Consumable, 1));
        itemList.Add(new ItemData_hur(110, "������� ���", false, ItemData_hur.ItemType.head, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(111, "�������", false, ItemData_hur.ItemType.head, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(112, "�������", false, ItemData_hur.ItemType.head, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(113, "������", false, ItemData_hur.ItemType.booster, ItemData_hur.UsingType.Consumable, 1));
        itemList.Add(new ItemData_hur(114, "���� �ָӴ�", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(115, "ū �ָӴ�", false, ItemData_hur.ItemType.bag, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(116, "������", false, ItemData_hur.ItemType.weapon, ItemData_hur.UsingType.Wearable, 1));
        itemList.Add(new ItemData_hur(117, "��������", false, ItemData_hur.ItemType.weapon, ItemData_hur.UsingType.Wearable, 1));
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
}
