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

    //주변 슬롯의 아이템을 디아블로식 인벤토리에 넣으면 사라짐
    [SerializeField] private Transform DiabloSpace;
    
    public bool ItemDragOn = false;

    private void Update()
    {
        KnowItem();
    }

    private void KnowItem()
    //아이템을 감지했을 때 프리팹 생성
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ItemData_hur randomItem = GetRandomItem();
            GameObject SurSlotPrefab = Instantiate(slotprefab, parentTransform);
            UpdateSlot(SurSlotPrefab, randomItem);
        }
    }

    private ItemData_hur GetRandomItem()
    //나중에 수정할 것 - itemList에 해당하는 아이템과 이어져야 함
    {
        //int randomindex = Random.Range(0, database.itemList.Count);
        int Itemindex = Random.Range(0, database.itemList.Count);

        Debug.Log("랜덤 돌리는 중..."); 
        
        return database.itemList[Itemindex];
    }
    private void UpdateSlot(GameObject prefab, ItemData_hur itemdata)
    //아이템이 랜덤으로 뜰 때 아이템 이름과 이미지가 맞게 만드는 함수
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

