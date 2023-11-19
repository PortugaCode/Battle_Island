using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Slot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private InventoryController_C IvnCont;
    [SerializeField] private SlotManager slotManager;
    [SerializeField] private InventoryHighlight_C ih;

    public int id;
    public bool grapItem;
    private void Awake()
    {
        IvnCont = FindObjectOfType<InventoryController_C>();
        slotManager = FindObjectOfType<SlotManager>();
        ih = FindObjectOfType<InventoryHighlight_C>();

        grapItem = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!grapItem)
        {
            grapItem = true;

            Debug.Log($"{id}");
            IvnCont.matchingID = id;
            IvnCont.CreateItem();
            ih.itemID = id;
            slotManager.ItemID.Remove(id);
            Destroy(gameObject);
        }
        
    }
}

