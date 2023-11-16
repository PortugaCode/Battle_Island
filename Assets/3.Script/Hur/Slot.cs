using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Slot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private InventoryController_C IvnCont;
    [SerializeField] private SlotManager slotManager;

    public int id;
    private void Awake()
    {
        IvnCont = FindObjectOfType<InventoryController_C>();
        slotManager = FindObjectOfType<SlotManager>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
<<<<<<< Updated upstream
        Debug.Log($"{id}");
        IvnCont.matchingID = id;
        IvnCont.CreateItem();
        slotManager.ItemID.Remove(id);
        Destroy(gameObject);
=======
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
        newSlot.transform.position = slotHolder.position + new Vector3(count * 0f, 0f, 0f);
    }

    private void GiveData(int _itemID) //수정
    {
        ItemData_hur newItem = database.GetItemByID(_itemID);

        if (newItem != null)
        {
            //newItem.itemID = _itemID; 
        }
        else
        {
            //Debug.Log("해당 ID값을 가진 아이템은 없습니다");
        }
>>>>>>> Stashed changes
    }
}

