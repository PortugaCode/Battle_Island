using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemData_hur
{
    public Sprite itemIcon;
    public int itemID;
    public string itemName;
    public bool isUsed;
    public int itemCount;
    
    public ItemType itemType; // ������ ���� Ÿ��
    public UsingType usingType; // ������ ���� Ÿ��

    public enum ItemType
    {
        medic, booster, head, armor, bag, weapon, etc //ġ����, �ν���, ���, �׿�(�Ѿ�,����ź) 
    }

    public enum UsingType
    {
        Consumable, Wearable
    }

    public ItemData_hur(int _itemID, string _itemName, bool _isUsed, ItemType _itemType, UsingType _usingType, int _count)
        //�ʱ�ȭ
    {
        itemID = _itemID;
        itemName = _itemName;
        isUsed = _isUsed;
        itemType = _itemType;
        usingType = _usingType;
        itemIcon = Resources.Load("resource_hur/" + _itemID.ToString(), typeof(Sprite)) as Sprite;
        itemCount = _count;
    }
    public ItemData_hur CreateNewItemData(int _itemID, string _itemName, bool _isUsed, ItemType _itemType, UsingType _usingType, int _count)
    {
        return new ItemData_hur(_itemID, _itemName, _isUsed, _itemType, _usingType, _count);
    }

}
