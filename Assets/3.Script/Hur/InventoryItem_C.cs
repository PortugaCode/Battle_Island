using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem_C : MonoBehaviour
{
    //이 스크립트는 Diablo식 인벤토리에 둘 아이템의 크기를 결정함

    public ItemData_C itemData;

    public int HEIGHT
    {
        get
        {
            if (rotated == false)//회전이 아니라면 높이는 높이
            {
                return itemData.height;
            }
            return itemData.width;//회전을 했다면 높이 -> 넓이로 변환
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

    public void Set(ItemData_C itemData)
    {
        this.itemData = itemData;

        GetComponent<Image>().sprite = itemData.itemIcon;//이거 원래는 누구의 itemIcon임? 원래 C거

        //무조건 주변 인벤토리에서 끌어온 아이템과 같아야 함
        Debug.Log("아이템 뿅");
        Debug.Log($"Diablo에 전해진 정보 : {PlayerPrefs.GetInt("Item_ID")}");

        Vector2 size = new Vector2();
        size.x = itemData.width * ItemGrid_C.tileSizeWidth;
        size.y = itemData.height * ItemGrid_C.tileSizeHeight;
        GetComponent<RectTransform>().sizeDelta = size;

    }

    public void Rotate()
    {
        rotated = !rotated;
        RectTransform rect = GetComponent<RectTransform>();
        rect.rotation = Quaternion.Euler(0, 0, rotated == true ? 90f : 0f);
    }
}
