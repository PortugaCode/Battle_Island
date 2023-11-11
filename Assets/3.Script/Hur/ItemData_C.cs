using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ItemData_C : ScriptableObject
{
    public int width = 1;
    public int height = 1;
    public Sprite itemIcon;

    public int itemID;//itemData_hur�� itemID�� ����.
    public string itemName;
    public bool isUsed; //����߳���?
    public int itemCount;

    //public ItemType itemType; // ������ ���� Ÿ��
    //public UsingType usingType; // ������ ���� Ÿ��

    //public enum ItemType
    //{
    //    medic, booster, head, armor, bag, weapon, etc //ġ����, �ν���, ���, �׿�(�Ѿ�,����ź) 
    //}

    //public enum UsingType
    //{
    //    Consumable, Wearable
    //}
    //public ItemData_C(int _itemID, string _itemName, bool _isUsed, ItemType _itemType, UsingType _usingType, int _count)
    ////�ʱ�ȭ
    //{
    //    itemID = _itemID;
    //    itemName = _itemName;
    //    isUsed = _isUsed;
    //    itemType = _itemType;
    //    usingType = _usingType;
    //    itemIcon = Resources.Load("resource_hur/" + _itemID.ToString(), typeof(Sprite)) as Sprite;
    //    itemCount = _count;
    //}
}
