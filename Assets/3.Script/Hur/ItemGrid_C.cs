using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemGrid_C : MonoBehaviour
{
    //�̰� �����ϸ� �����۵� Ŀ����
    /*
     *  32*32ĭ�̸� Grid Imagetype�� 1, GridSizeWidth�� 17, Height�� 28
     *  64*64ĭ��  Grid Imagetype�� 0.5, GridSizeWidth�� 8, Height�� 14
     * 
     */
    
    public const float tileSizeWidth = 32f;
    public const float tileSizeHeight = 32f;

    InventoryItem_C[,] inventoryItemSlot; // [,] : ��� ���� ���� 2���� �迭

    RectTransform rectTransform;

    [SerializeField] private int gridSizeWidth = 17;
    [SerializeField] private int gridSizeHeight = 28;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);
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
                inventoryItemSlot[item.onGridPositionX + ix, item.onGridPositionY + iy] = null;
            }
        }
    }

    private void Init(int width, int height)
    {
        inventoryItemSlot = new InventoryItem_C[width, height];
        Vector2 size = new Vector2(width * tileSizeWidth,
            height * tileSizeHeight);
        rectTransform.sizeDelta = size;//sizeDelta : RectTransform ũ�⸦ �����ϴ� �Ӽ� �� �ϳ�
    }

    internal InventoryItem_C GetItem(int x, int y)
    {
        return inventoryItemSlot[x, y];
    }

    Vector2 positionOnTheGrid = new Vector2(); //���콺 �������� ��ġx

    public Vector2Int? FindSpaceForObject(InventoryItem_C itemToInsert)
    {
        int height = gridSizeHeight - itemToInsert.HEIGHT + 1;
        int width = gridSizeWidth - itemToInsert.WIDTH + 1;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (CheckAvailableSpace(x, y,
                    itemToInsert.WIDTH,
                    itemToInsert.HEIGHT
                    ))
                {
                    return new Vector2Int(x, y);
                }

            }
        }

        return null;
    }

    Vector2Int tileGridPosition = new Vector2Int();
    //Ÿ�� �׸��忡���� ��ġ
    //Vector2Int�� ������ �� 2D����
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        //mousePosition = ȭ��� ���콺 ��ġ
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
    }

    public bool PlaceItem(InventoryItem_C inventoryItem, int posX, int posY, ref InventoryItem_C overlapItem)
    {
        if (!BoundryCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT))
        {
            return false;
        }

        if (OverlapCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT, ref overlapItem) == false)
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem != null)
        {
            CleanGridReference(overlapItem);
        }

        PlaceItem(inventoryItem, posX, posY);

        return true;
    }

    public void PlaceItem(InventoryItem_C inventoryItem, int posX, int posY)
    {
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        for (int x = 0; x < inventoryItem.WIDTH; x++)
        {
            for (int y = 0; y < inventoryItem.HEIGHT; y++)
            {
                inventoryItemSlot[posX + x, posY + y] = inventoryItem;
            }
        }

        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;

        Vector2 position = CalculatePositionOnGRid(inventoryItem, posX, posY);

        rectTransform.localPosition = position;
    }

    public Vector2 CalculatePositionOnGRid//�κ��丮 �� ������ ��ġ
        (InventoryItem_C inventoryItem, int posX, int posY)
    {
        Vector2 position = new Vector2();
        position.x =
            posX * tileSizeWidth + tileSizeWidth * inventoryItem.WIDTH / 2;

        position.y =
            -(posY * tileSizeHeight + tileSizeHeight * inventoryItem.HEIGHT / 2);

        return position;
    }

    private bool OverlapCheck(int posX, int posY, int width, int height,
        ref InventoryItem_C overlapItem)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (inventoryItemSlot[posX + x, posY + y] != null)
                {
                    if (overlapItem == null)
                    {
                        overlapItem = inventoryItemSlot[posX + x, posY + y];
                    }
                    else
                    {
                        if (overlapItem != inventoryItemSlot[posX + x, posY + y])
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    private bool CheckAvailableSpace(int posX, int posY, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (inventoryItemSlot[posX + x, posY + y] != null)
                {
                    return false; //���� ��ø�� ����
                }
            }
        }

        return true; //���� ��ø�� ����
    }

    bool PositionCheck(int posX, int posY)
    {
        if (posX < 0 || posY < 0)
        {
            return false;
        }
        if (posX >= gridSizeWidth || posY >= gridSizeHeight)
        {
            return false;
        }

        return true;
    }

    public bool BoundryCheck(int posX, int posY, int width, int height)
    {
        if (PositionCheck(posX, posY) == false)
        {
            return false;
        }

        posX += width - 1;
        posY += height - 1;

        if (PositionCheck(posX, posY) == false)
        {
            return false;
        }

        return true;
    }
}
