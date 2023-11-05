using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/*
   * ������ = �� 19��
   *   - 1. �Ҹ�ǰ
   *          (1) ġ���� - ���޻���(Ǯü�� �� 80�۸� ġ��)
   *          (2) �ν��� - �Ƶ巹���� �ֻ��(�ӵ� ����), ������(Ǯü��ĭ ����)
   *          (3) �׿� - �Ѿ˹ڽ�, ����ź
   *          
   *   - 2. ���
   *          (1) ��� - ����������, �������, �������
   *          (2) �� - �������� ����1, 2, 3
   *          (3) �賶 - �賶 ����1, 2, 3, �Ŀ�ġ1, 2
   *          (4) ���� - ������, ��������
   *          
   *   �κ��丮��UI, ������(�غ� â��), ������� �־�� ��
   * 
   *  ������ �Ӽ�
   *      ����� �Ƴ� �ȵƳ�
   *      �Ҹ�Ǵ� ���ΰ�? - bool
   *          ��ŭ �Ҹ�Ǿ���? �ۼ�Ʈ
   *      ���Ǵ� ���ΰ�? - bool
   *          ��� ��� �ǳ�?
   *      � ȿ���� ���̴���
   *          ȿ���� ���� ������
   *          ������ ���ݷ� 
   *          �Ƶ巹���� - �ӵ�
   *          ������ - Ǯü��ĭ ����
   *          ���濡 ���� ĭ�� ��ȭ (�κ��丮 �˾�â���� �ؾ��ϳ�?)
   *          
   *      ������ - bool
   *      ������ �Ƴ� �ȵƳ� - bool
   *      �Է°�
   */
public class ItemData_hur
{
    public bool notice; //����
    public Sprite itemIcon;
    public int itemID;
    public string itemName;
    public bool isUsed; //����߳���?
    
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

    public ItemData_hur(bool _notice, Sprite _itemIcon, int _itemID, string _itemName, bool _isUsed, ItemType _itemType, UsingType _usingType)
    {
        notice = _notice;
        itemIcon = _itemIcon;
        itemID = _itemID;
        itemName = _itemName;
        isUsed = _isUsed;
        itemType = _itemType;
        usingType = _usingType;
    }
}
