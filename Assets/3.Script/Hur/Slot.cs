using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotHolder; //슬롯 부모
    [SerializeField] private PlayerController player;
    [SerializeField] private Database_hur database;
    //[SerializeField] private NoticeItem notice;
    [SerializeField] private ItemData_hur ItemData;

    public int count; //초기 슬롯 개수
    //private int _itemID;

    private new List<int> ItemID = new List<int>();

    private void Awake()
    {
        ItemID.Add(108);
        ItemID.Add(104);
        ItemID.Add(105);
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        database = FindObjectOfType<Database_hur>();
        //notice = FindObjectOfType<NoticeItem>();
       
    }
    private void Update()
    {
        //if (player.getItem)
        //{
        //    player.getItem = false;
        //    //MeetItem();
        //}
        
    }

    public void AddSlot()
    {
        for (int i = 0; i < ItemID.Count; i++)
        {
            MeetItem(ItemID[i]);

        }
    }

    private void MeetItem(int id) //아이템 감지하면 슬롯개수 ++
    {
        count++;
        PrefabPlus(count, id);
    }

    private void PrefabPlus(int count, int id)
    {
        Debug.Log("PrefabPlus");
        GameObject newSlot = Instantiate(slotPrefab, transform.position, Quaternion.identity);
        newSlot.transform.SetParent(slotHolder);
        newSlot.transform.position = slotHolder.position + new Vector3(count * 0f, 0f, 0f);

        // id 받아왔잖아 -> 했어
        // 그 id에 맞는 인덱스 알아냇어
        // 그 인덱스 (너가 만들었ㅅ던 아이템 리스트에서 지금 id와 일치하는 인덱스) 알아
        // newSlot에 띄울 이름, 아이콘<= 이거 어딨냐 너가 만들었던 아이템 리스트에 있잔아
        // 

        int index = database.ReturnItemIndex(id);
        newSlot.transform.GetChild(0).GetComponent<Image>().sprite = database.itemList[index].itemIcon;
        newSlot.transform.GetChild(2).GetComponent<Text>().text = database.itemList[index].itemName;

    }


}

