using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ItemData_C : ScriptableObject
{
    public int width = 1;
    public int height = 1;
    public Sprite itemIcon;

    public bool notice; //����
    public int itemID;
    public string itemName;
    [SerializeField] private GameObject itemPrefab02;
    public bool isUsed; //����߳���?
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
}
