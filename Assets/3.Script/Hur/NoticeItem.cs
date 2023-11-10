using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NoticeItem : MonoBehaviour
{
    [SerializeField] private Database_hur database;
    [SerializeField] private GameObject slotprefab;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private PlayerController player;
    [SerializeField] private ItemTrigger itemTrigger;

    public bool ItemDragOn = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        database = FindObjectOfType<Database_hur>();
        itemTrigger = FindObjectOfType<ItemTrigger>();
    }
    private void Update()
    {
        if (player.eatItem)
        {   
            KnowItem();
        }
        
    }

    private void KnowItem()
    //아이템을 감지했을 때 슬롯 프리팹 생성
    {
        player.eatItem = false;

        //만약 감지했던 아이템 데이터가 Surround 인벤토리에 뜬다면
        Debug.Log("아이템 감지 in NoticeItem");

        //ItemData_hur randomItem = GetRandomItem();
        //GameObject SurSlotPrefab = Instantiate(slotprefab, parentTransform);
        //UpdateSlot(SurSlotPrefab, randomItem);

        ItemData_hur itemData = MatchInfo();
        GameObject SurSlotPrefab = Instantiate(slotprefab, parentTransform);
        UpdateSlot(SurSlotPrefab, itemData);
    }

    private ItemData_hur MatchInfo()
    //itemList에 해당하는 아이템과 이어져야 함
    {
        //int Itemindex = Random.Range(0, database.itemList.Count);
        ////Debug.Log("아이템 습득!");
        //return database.itemList[Itemindex];

        int Itemindex = itemTrigger.PlayerPrefs.SetInt;
        Debug.Log("아이템 습득!");
        return database.itemList[Itemindex];
    }
    private void UpdateSlot(GameObject prefab, ItemData_hur itemdata)
    //아이템이 랜덤으로 뜰 때 슬롯의 아이템 이름과 슬롯의 이미지가 맞게 만드는 함수
    {
        Text itemNameText = prefab.GetComponentInChildren<Text>();
        Image itemImage = prefab.GetComponentInChildren<Image>();

        if (itemNameText != null)
        {
            itemNameText.text = itemdata.itemName;
        }

        if (itemImage != null)
        {
            itemImage.sprite = itemdata.itemIcon;
        }
    }
}

