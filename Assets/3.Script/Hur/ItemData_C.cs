using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ItemData_C : ScriptableObject
{
    public int width = 1;
    public int height = 1;
    public Sprite itemIcon;

    public int itemID;//itemData_hur의 itemID와 같다.
    public string itemName;
    public bool isUsed; //사용했나요?
    public int itemCount;

    //public ItemType itemType; // 아이템 종류 타입
    //public UsingType usingType; // 아이템 쓸모 타입

    //public enum ItemType
    //{
    //    medic, booster, head, armor, bag, weapon, etc //치료제, 부스터, 장비, 그외(총알,수류탄) 
    //}

    //public enum UsingType
    //{
    //    Consumable, Wearable
    //}
    //public ItemData_C(int _itemID, string _itemName, bool _isUsed, ItemType _itemType, UsingType _usingType, int _count)
    ////초기화
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
