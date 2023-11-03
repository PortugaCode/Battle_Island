using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid_C))]
public class GridInteract_C : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    InventoryController_C inventoryController;
    private ItemGrid_C itemGrid;
    private void Awake()
    {
        inventoryController =
            FindObjectOfType(typeof(InventoryController_C)) as InventoryController_C;
        itemGrid = GetComponent<ItemGrid_C>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = itemGrid;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = null;
    }

}
