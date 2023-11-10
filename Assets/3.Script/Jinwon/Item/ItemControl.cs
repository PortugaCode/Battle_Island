using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemControl : MonoBehaviour
{
    [Header("Item ID")]
    public string itemName;
    public int id;

    [Header("Status")]
    public bool canGet = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canGet = true;
            InventoryControl.instance.focusedItem = gameObject;
            UIManager.instance.ShowGetItemUI(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (InventoryControl.instance.focusedItem == gameObject)
            {
                canGet = false;
                InventoryControl.instance.focusedItem = null;
                UIManager.instance.CloseGetItemUI();
            }
        }
    }

    public void PickUpItem()
    {
        if (!canGet)
        {
            return;
        }

        UIManager.instance.CloseGetItemUI();

        InventoryControl.instance.GetItem(itemName, id);

        Destroy(gameObject);
    }
}
