using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem_C : MonoBehaviour
{
    [SerializeField] private Database_hur database;
    public ItemData_C itemData;

    private void Start()
    {
        database = FindObjectOfType<Database_hur>();
    }
    
    public int HEIGHT
    {
        get
        {
            if (rotated == false)
            {
                return itemData.height;
            }
            return itemData.width;
        }
    }
    public int WIDTH
    {
        get
        {
            if (rotated == false)
            {
                return itemData.width;
            }
            return itemData.height;
        }
    }

    public int onGridPositionX;
    public int onGridPositionY;

    public bool rotated = false;

    internal void Set(ItemData_C itemData)
    {
        this.itemData = itemData;
        GetComponent<Image>().sprite = itemData.itemIcon;

        //무조건 주변 인벤토리의 슬롯과 같아야 함
        Debug.Log("아이템 뿅");

        Vector2 size = new Vector2();
        size.x = itemData.width * ItemGrid_C.tileSizeWidth;
        size.y = itemData.height * ItemGrid_C.tileSizeHeight;
        GetComponent<RectTransform>().sizeDelta = size;

    }

    internal void Rotate()
    {
        rotated = !rotated;
        RectTransform rect = GetComponent<RectTransform>();
        rect.rotation = Quaternion.Euler(0, 0, rotated == true ? 90f : 0f);
    }
}
