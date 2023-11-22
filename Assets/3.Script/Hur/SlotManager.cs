using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotHolder; //슬롯 부모
    [SerializeField] private Database_hur database;
    [SerializeField] private ItemData_hur ItemData;

    public int count; //초기 슬롯 개수

    public int id;

    private void Start()
    {
        //player = FindObjectOfType<PlayerController>();
        database = FindObjectOfType<Database_hur>();
    }

    public void AddSlot()
    {
        for (int i = 0; i < InventoryControl.instance.nearItemList.Count; i++)
        {
            MeetItem(InventoryControl.instance.nearItemList[i].GetComponent<ItemControl>().id);
        }
    }

    public void MeetItem(int id) //아이템 감지하면 슬롯개수++
    {
        count++;
        PrefabPlus(count, id);
    }

    private void PrefabPlus(int count, int id)
    {
        //주변 슬롯
        GameObject newSlot = Instantiate(slotPrefab, transform.position, Quaternion.identity);
        newSlot.transform.SetParent(slotHolder);
        newSlot.transform.position = slotHolder.position + new Vector3(count * 0f, 0f, 0f);

        //주변 슬롯
        int index = database.ReturnItemIndex(id);
        newSlot.transform.GetChild(0).GetComponent<Image>().sprite = database.itemList[index].itemIcon;
        newSlot.transform.GetChild(2).GetComponent<Text>().text = database.itemList[index].itemName;
        newSlot.GetComponent<Slot>().id = database.itemList[index].itemID;
        newSlot.GetComponent<Slot>().width = database.itemList[index].width;
        newSlot.GetComponent<Slot>().height = database.itemList[index].height;

        //아이템 타입
        //newSlot.GetComponent<Slot>().usingType = database.itemList[index].usingType;
        
        //CheckType(id);
    }
}
