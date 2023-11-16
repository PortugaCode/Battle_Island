using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight_C : MonoBehaviour
{ 
    [SerializeField] RectTransform hightlighter;
    public bool sinho = false;
    public void Show(bool b)
    {
        hightlighter.gameObject.SetActive(b);
    }

    public void SetSize(InventoryItem_C targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.WIDTH * ItemGrid_C.tileSizeWidth;
        size.y = targetItem.HEIGHT * ItemGrid_C.tileSizeHeight;

        hightlighter.sizeDelta = size;
    }

    public void SetPosition(ItemGrid_C targetGrid, InventoryItem_C targetItem)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGRid(
            targetItem,
            targetItem.onGridPositionX,
            targetItem.onGridPositionY);

        if (!sinho)
        {
            Debug.Log("하이라이트 ON"); //인벤토리 grid에 닿을 때마다 뜸 (같은 아이템도 마찬가지)
            Debug.Log("X좌표, Y좌표 : " + 
                (targetItem.onGridPositionX, targetItem.onGridPositionY));
            
            sinho = true;
            
        }
        
        hightlighter.localPosition = pos;
    }

    public void SetParent(ItemGrid_C targetGrid)
    {
        if (targetGrid == null)
        {
            return;
        }

        hightlighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }

    public void SetPosition(ItemGrid_C targetGrid, InventoryItem_C targetItem,
        int posX, int posY)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGRid(targetItem, posX, posY);

        hightlighter.localPosition = pos;
    }
}
