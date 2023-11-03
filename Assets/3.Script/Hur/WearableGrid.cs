using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WearableGrid : MonoBehaviour
{
    public const float tileSizeWidth = 32f;
    public const float tileSizeHeight = 32f;

    InventoryItem_C[,] inventoryItemSlot; // [,] : 행과 열을 가진 2차원 배열

    RectTransform rectTransform;

    [SerializeField] private int gridSizeWidth = 1;
    [SerializeField] private int gridSizeHeight = 1;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public InventoryItem_C PickUpItem(int x, int y)
    {
        InventoryItem_C toReturn = inventoryItemSlot[x, y];

        if (toReturn == null)
        {
            return null;
        }

        CleanGridReference(toReturn);

        return toReturn;
    }

    private void CleanGridReference(InventoryItem_C item)
    {
        for (int ix = 0; ix < item.WIDTH; ix++)
        {
            for (int iy = 0; iy < item.HEIGHT; iy++)
            {
                inventoryItemSlot[item.onGridPositionX + ix,
                    item.onGridPositionY + iy] = null;
            }
        }
    }
}
