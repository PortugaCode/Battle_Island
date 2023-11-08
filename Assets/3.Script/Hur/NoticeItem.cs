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
    [SerializeField] private SurrItemClick surItemClick;

    //주변 슬롯의 아이템을 디아블로식 인벤토리에 넣으면 사라짐
    [SerializeField] private Transform DiabloSpace;
    
    public bool ItemDragOn = false;

    private void Start()
    {
        surItemClick = FindObjectOfType<SurrItemClick>();
    }
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
        int randomindex = Random.Range(0, database.itemList.Count);
        return database.itemList[randomindex];
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

        //새로운 리스트에 넣기 - 나중에 제거하기 쉬움
        

    }
    public void ClickItem()
    {
        if (true)
        {
            surItemClick.Click = false;
            ItemDragOn = true;

            //주변슬롯아이템에 클릭하는 순간 밑에 함수 발동
            DestroyInSurr();
            ClickMouse();

        }
        //else
        //집은 채로 또 들려고 하면 주의문구 뜸
        //{
        //    Debug.Log("ItemDragOn이 아직 false가 아닙니다");
        //}
    }
    public void DestroyInSurr()
    //만약 디아 인벤토리Transform에 두면 해당 프리팹만 삭제된다. 
    {
        //Debug.Log("DestroyInSurr 입성!");
        
    }
    public void ClickMouse()
    {
        //Debug.Log("ClickMouse 입성!");
    }
}

