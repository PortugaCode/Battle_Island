using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Text itemName_txt;
    public Text itemCount_txt;

    public void AddItem(ItemData_hur _item)
    {
        itemName_txt.text = _item.itemName;
        icon.sprite = _item.itemIcon;
        
    }
}
