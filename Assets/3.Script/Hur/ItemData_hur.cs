using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/*
   * 아이템 = 총 19개
   *   - 1. 소모품
   *          (1) 치료제 - 구급상자(풀체력 중 80퍼만 치료)
   *          (2) 부스터 - 아드레날린 주사기(속도 증가), 진통제(풀체력칸 증가)
   *          (3) 그외 - 총알박스, 수류탄
   *          
   *   - 2. 장비
   *          (1) 헬멧 - 오토바이헬멧, 군용헬멧, 용접헬멧
   *          (2) 옷 - 군용조끼 레벨1, 2, 3
   *          (3) 배낭 - 배낭 레벨1, 2, 3, 파우치1, 2
   *          (4) 무기 - 라이플, 스나이퍼
   *          
   *   인벤토리형UI, 아이콘(준비 창에), 장착기능 넣어야 함
   * 
   *  아이템 속성
   *      사용이 됐나 안됐나
   *      소모되는 것인가? - bool
   *          얼만큼 소모되었나? 퍼센트
   *      장비되는 것인가? - bool
   *          어디에 장비가 되나?
   *      어떤 효과를 보이는지
   *          효과가 없는 아이템
   *          무기라면 공격력 
   *          아드레날린 - 속도
   *          진통제 - 풀체력칸 증가
   *          가방에 따라 칸수 변화 (인벤토리 팝업창으로 해야하나?)
   *          
   *      버리기 - bool
   *      감지가 됐나 안됐나 - bool
   *      입력값
   */
public class ItemData_hur
{
    public bool notice; //감지
    public Sprite itemIcon;
    public int itemID;
    public string itemName;
    public bool isUsed; //사용했나요?
    
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
