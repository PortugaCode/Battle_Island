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
            InventoryControl.instance.focusedItems.Add(gameObject);
            UIManager.instance.ShowGetItemUI(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canGet = false;
            InventoryControl.instance.focusedItems.Remove(gameObject);

            if (InventoryControl.instance.focusedItems.Count == 0)
            {
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

        InventoryControl.instance.focusedItems.RemoveAt(InventoryControl.instance.focusedItems.Count - 1);

        if (InventoryControl.instance.focusedItems.Count == 0)
        {
            UIManager.instance.CloseGetItemUI();
        }
        else
        {
            UIManager.instance.ShowGetItemUI(InventoryControl.instance.focusedItems[InventoryControl.instance.focusedItems.Count - 1]);
        }

        InventoryControl.instance.GetItem(itemName, id);

        Destroy(gameObject);
    }
}
