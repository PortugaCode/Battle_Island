using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ItemData_C : ScriptableObject
{
    public int width = 1;
    public int height = 1;
    public Sprite itemIcon;

    public bool notice; //감지
    public int itemID;
    public string itemName;
    [SerializeField] private GameObject itemPrefab02;
    public bool isUsed; //사용했나요?
    public int itemCount;

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
}
