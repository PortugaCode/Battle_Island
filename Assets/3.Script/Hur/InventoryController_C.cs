using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController_C : MonoBehaviour
{
    [SerializeField] private ItemGrid_C selectedItemGrid;

    InventoryItem_C selectedItem;
    InventoryItem_C overlapItem;
    RectTransform rect;

    [SerializeField] List<ItemData_C> item;
    [SerializeField] GameObject itemPrefab;

    [SerializeField] Transform canvasT;

    private InventoryHighlight_C inventoryHighlight;

    public bool grabItem = false; // ���� ���콺�� �������� ����ִ°�?
    public bool isOverlap = false; // �κ��丮�� �������� ���� �� ��ġ�°�?

    public int itemID;
    //private Slot slot;

    // �ֺ� ����
    public Slot[] slots;
    public Transform slotHolder;

    // ������ ���̶���Ʈ
    Vector2Int oldPosition;
    InventoryItem_C itemToHighlight;

    // [�κ��丮] 2���� �迭 ������
    public int[,] array = new int[17, 28];

    // ���� ����
    public int currentWidth;
    public int currentHeight;

    public ItemGrid_C SelectedItemGrid
    {
        get => selectedItemGrid;
        set
        {
            selectedItemGrid = value;
            inventoryHighlight.SetParent(value);
        }
    }
    private void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight_C>();
        slots = slotHolder.GetComponentsInChildren<Slot>();
        //slot = GetComponent<Slot>();
    }
    private void Update()
    {
        if (selectedItemGrid == null)
        {
            inventoryHighlight.Show(false);
            return;
        }

        ItemIconDrag(); // [�ֺ�]���� ������ ������ Ŭ�� �� ������ �������� ���콺�� ����ٴϴ� �Լ�

        HandleHighlight(); // [�κ��丮]���� ������ ���� ���콺 �÷����� ������ ���̶���Ʈ ǥ���ϴ� �Լ�

        if (Input.GetMouseButtonDown(1)) // ���콺 ��Ŭ��
        {
            RotateItem(); // ������ ȸ���ϴ� �Լ�
        }

        if (Input.GetMouseButtonDown(0)) // ���콺 ��Ŭ��
        {
            LeftMouseButtonPress();
        }
    }

    private void ItemIconDrag() // [�ֺ�]���� ������ ������ Ŭ�� �� ������ �������� ���콺�� ����ٴϴ� �Լ�
    {
        if (selectedItem != null)
        {
            //Debug.Log($"{selectedItem.itemData.name} �巡�� ��");
            rect.position = Input.mousePosition;
        }
    }

    private void RotateItem() // ������ ȸ���ϴ� �Լ�
    {
        if (selectedItem == null)
        {
            return;
        }

        selectedItem.Rotate();
    }

    private void HandleHighlight() // [�κ��丮]���� ������ ���� ���콺 �÷����� ������ ���̶���Ʈ ǥ���ϴ� �Լ�
    {
        Vector2Int positionOnGrid = GetTileGridPosition();

        if (oldPosition == positionOnGrid)
        {
            return;
        }

        oldPosition = positionOnGrid;

        if (selectedItem == null)
        {
            itemToHighlight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            if (itemToHighlight != null)
            {
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighlight);
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight, itemID);
            }
            else
            {
                inventoryHighlight.Show(false);
            }
        }
        else
        {
            inventoryHighlight.Show(selectedItemGrid.BoundryCheck(positionOnGrid.x, positionOnGrid.y, selectedItem.WIDTH, selectedItem.HEIGHT));
            inventoryHighlight.SetSize(selectedItem);
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    public void CreateItem(int id) // [�ֺ�]���� �������� Ŭ������ �� ȣ��Ǵ� �Լ�
    {
        grabItem = true;

        InventoryItem_C inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem_C>();
        selectedItem = inventoryItem;

        itemID = id;

        rect = inventoryItem.GetComponent<RectTransform>();
        rect.SetParent(canvasT);
        rect.SetAsLastSibling();

        inventoryItem.Set(item[id - 100]);
    }

    private void LeftMouseButtonPress() // ���� ���콺 ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    {
        Vector2Int tileGridPosition = GetTileGridPosition();

        if (selectedItem == null) // ���� ������ �������� ������ ������ �Ⱦ�
        {
            PickUpItem(tileGridPosition);
        }
        else // �̹� ������ �������� ������ ������ ��������
        {
            PlaceItem(tileGridPosition);
        }
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (selectedItem != null)
        {
            position.x -= (selectedItem.WIDTH - 1) * ItemGrid_C.tileSizeWidth / 2;
            position.y += (selectedItem.HEIGHT - 1) * ItemGrid_C.tileSizeHeight / 2;

        }

        return selectedItemGrid.GetTileGridPosition(position);
    }

    private void PlaceItem(Vector2Int tileGridPosition) // ������ ��������
    {
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);

        if (complete)
        {
            if (overlapItem != null) // �������� �������� ���� �̹� �������� �� ���� ��
            {
                Debug.Log("Overlap �߻�");

                DeleteItem(overlapItem); // �ؿ� �ִ� ������ ����

                InsertItem(tileGridPosition.x, tileGridPosition.y, itemID); // ����ִ� ������ �ְ�

                selectedItem = overlapItem; // ������ �����ϰ�
                currentWidth = selectedItem.itemData.width; // width �ٲ��ְ�
                currentHeight = selectedItem.itemData.height; // height �ٲ��ְ�
                itemID = selectedItem.itemData.itemID; // itemID �ٲ��ְ�

                overlapItem = null;
                rect = selectedItem.GetComponent<RectTransform>();
                rect.SetAsLastSibling();
            }
            else
            {
                InsertItem(tileGridPosition.x, tileGridPosition.y, itemID);
                selectedItem = null;
            }
        }
    }

    private void PickUpItem(Vector2Int tileGridPosition) // [�κ��丮]���� ������ Ŭ���Ͽ� �Ⱦ��� �� ȣ��Ǵ� �Լ�
    {
        //Debug.Log("[�κ��丮]���� ������ �Ⱦ�");

        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);

        if (selectedItem == null)
        {
            return;
        }

        currentWidth = selectedItem.itemData.width;
        currentHeight = selectedItem.itemData.height;

        if (selectedItem != null)
        {
            rect = selectedItem.GetComponent<RectTransform>();
        }

        DeleteItem(selectedItem);
        //Calculate(tileGridPosition.x, tileGridPosition.y, itemID);
    }

    // ----------------------------------------------------------------------------------

    public void GridCheck() // �׸���(2���� �迭) �������� ����ִ� ��ġ ���
    {
        int count = 0;

        // ���� 17, ���� 28
        for (int i = 0; i < 17; i++)
        {
            for (int j = 0; j < 28; j++)
            {
                if (array[i, j] != 0)
                {
                    count += 1;
                    //Debug.Log($"Ȯ�� : {i},{j} = {array[i, j]}");
                }
            }
        }

        Debug.Log("��ƺ�� �κ��丮 : " + count);
    }

    /*public void Calculate(int x, int y, int itemID)
    {
        if (grabItem) // �������� ��� ���� �� array�� itemID �ֱ�
        {
            for (int i = 0; i < currentWidth; i++)
            {
                for (int j = 0; j < currentHeight; j++)
                {
                    array[x + i, y + j] = itemID;
                }
            }

            grabItem = false;
        }
        else // �������� �� ��� ���� �� array���� itemID ����
        {
            // �ֱ�� �Ǵµ� �� ����� �Ȼ�����?
            // (Ŭ���� ��ġ���� ������ ��縸ŭ ������ ����)
            // �׷��� �������� �� ������ Ŭ�� ��ġ�� �ƴ� �������� ���� ��� ��ǥ���� �����ؾ� �Ѵ�. (selectedItem.onGridPosition ����)
            
            for (int i = 0; i < currentWidth; i++)
            {
                for (int j = 0; j < currentHeight; j++)
                {
                    array[i + selectedItem.onGridPositionX, j + selectedItem.onGridPositionY] = 0;
                }
            }

            grabItem = true;
        }

        GridCheck();
    }*/

    public void InsertItem(int x, int y, int itemID) //������ �ѹ��� InsertItem�� DeleteItem �Լ��� ����
    {
        if (selectedItem == null)
        {
            Debug.Log("SelectedItem�� Null�̴�");
        }
        else
        {
            Debug.Log($"{selectedItem.itemData.itemName}�� array�� �ֽ��ϴ�");
        }

        for (int i = 0; i < currentWidth; i++)
        {
            for (int j = 0; j < currentHeight; j++)
            {
                array[x + i, y + j] = itemID;
            }
        }

        GridCheck();
    }

    public void DeleteItem(InventoryItem_C item)
    {
        Debug.Log($"{item.itemData.itemName}�� array���� �����մϴ�");

        // �ֱ�� �Ǵµ� �� ����� �Ȼ�����?
        // (Ŭ���� ��ġ���� ������ ��縸ŭ ������ ����)
        // �׷��� �������� �� ������ Ŭ�� ��ġ�� �ƴ� �������� ���� ��� ��ǥ���� �����ؾ� �Ѵ�. (selectedItem.onGridPosition ����)

        for (int i = 0; i < item.itemData.width; i++)
        {
            for (int j = 0; j < item.itemData.height; j++)
            {
                array[i + item.onGridPositionX, j + item.onGridPositionY] = 0;
            }
        }

        GridCheck();
    }

}
