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
    public int width;
    public int height;
    
    public ItemType itemType; // 아이템 종류 타입
    public UsingType usingType; // 아이템 쓸모 타입

    public enum ItemType
    {
        medic, booster, head, armor, bag, weapon, etc //치료제, 부스터, 장비, 그외(총알,수류탄) 
    }

    public enum UsingType
    {
        Consumable, Wearable
    }

    public ItemData_hur(int _itemID, string _itemName, bool _isUsed, ItemType _itemType, UsingType _usingType, int _count, int _width, int _height)
        //초기화
    {
        itemID = _itemID;
        itemName = _itemName;
        isUsed = _isUsed;
        itemType = _itemType;
        usingType = _usingType;
        itemIcon = Resources.Load("resource_hur/" + _itemID.ToString(), typeof(Sprite)) as Sprite;
        itemCount = _count;
        width = _width;
        height = _height;
    }
}
