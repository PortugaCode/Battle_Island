using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight_C : MonoBehaviour
{
    [SerializeField] RectTransform hightlighter;
    [SerializeField] private InventoryController_C inv_cont;

    private void Start()
    {
        inv_cont = FindObjectOfType<InventoryController_C>();
    }

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

    public void SetPosition(ItemGrid_C targetGrid, InventoryItem_C targetItem, int itemID)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGRid(targetItem, targetItem.onGridPositionX, targetItem.onGridPositionY);

        //시작 좌표
        int x = targetItem.onGridPositionX;
        int y = targetItem.onGridPositionY;

        //계산
        //inv_cont.Calculate(x, y, itemID);

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
