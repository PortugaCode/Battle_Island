using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Slot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private InventoryController_C IvnCont;
    [SerializeField] private SlotManager slotManager;
    [SerializeField] private InventoryHighlight_C ih;
    [SerializeField] private OpenInvent openIvn;

    public int id;
    public bool grapItem;

    public int width;
    public int height;
    public ItemData_hur.ItemType itemType;
    public ItemData_hur.UsingType usingType;

    private void Awake()
    {
        IvnCont = FindObjectOfType<InventoryController_C>();
        slotManager = FindObjectOfType<SlotManager>();
        ih = FindObjectOfType<InventoryHighlight_C>();
        openIvn = FindObjectOfType<OpenInvent>();

        grapItem = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!grapItem)
        {
            grapItem = true;

            IvnCont.CreateItem(id);
            IvnCont.currentWidth = width;
            IvnCont.currentHeight = height;
            Destroy(gameObject);
        }
        
    }
}

