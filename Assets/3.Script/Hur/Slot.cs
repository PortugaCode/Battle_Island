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
    [SerializeField] private NoticeItem notice;

    public int count; //초기 슬롯 개수
    private int _itemID;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        database = FindObjectOfType<Database_hur>();
        notice = FindObjectOfType<NoticeItem>();
    }
    private void Update()
    {
        if (player.getItem)
        {
            player.getItem = false;
            MeetItem();
        }

        GiveData(_itemID);
    }

    private void MeetItem() //아이템 감지하면 슬롯개수 ++
    {
        count++;
        PrefabPlus(count);
    }

    private void PrefabPlus(int count)
    {
        GameObject newSlot = Instantiate(slotPrefab);
        newSlot.transform.position = slotHolder.position + new Vector3(count * 0f, 0f, 5f);
    }

    private void GiveData(int _itemID) //수정
    {
        ItemData_hur newItem = database.GetItemByID(_itemID);

        if(newItem != null)
        {
            _itemID = newItem.itemID;
            //newItem.itemID = _itemID; 
        }
        else
        {
            //Debug.Log("해당 ID값을 가진 아이템은 없습니다");
        }
    }
}
