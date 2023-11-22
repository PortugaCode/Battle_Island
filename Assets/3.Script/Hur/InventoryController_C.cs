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

    public bool grabItem = false; // 현재 마우스에 아이템을 들고있는가?
    public bool isOverlap = false; // 인벤토리에 아이템을 놓을 때 겹치는가?
    private bool isNewItem = false; // place할 때 플레이어 인벤토리에 첫 추가인가?

    public int itemID;
    //private Slot slot;

    // 주변 슬롯
    public Slot[] slots;
    public Transform slotHolder;

    // 아이템 하이라이트
    Vector2Int oldPosition;
    InventoryItem_C itemToHighlight;

    // [인벤토리] 2차원 배열 데이터
    public int[,] array = new int[17, 28];

    // 슬롯 선택
    public int currentWidth;
    public int currentHeight;

    //플레이어 스크립트와 연동 (InsertItem)
    public int x;
    public int y;

    // 스크립트 참조
    private OpenInvent openInvent;
    
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
        openInvent = FindObjectOfType<OpenInvent>();
    }
    private void Update()
    {
        if (selectedItemGrid == null)
        {
            inventoryHighlight.Show(false);
            return;
        }

        ItemIconDrag(); // [주변]에서 아이템 아이콘 클릭 시 아이템 아이콘이 마우스를 따라다니는 함수

        if (openInvent.Open)
        {
            HandleHighlight(); // [인벤토리]에서 아이템 위에 마우스 올렸을때 형광색 하이라이트 표시하는 함수
        }

        if (Input.GetMouseButtonDown(1)) // 마우스 우클릭
        {
            RotateItem(); // 아이템 회전하는 함수
        }

        if (Input.GetMouseButtonDown(0)) // 마우스 좌클릭
        {
            LeftMouseButtonPress();
        }
    }

    private void ItemIconDrag() // [주변]에서 아이템 아이콘 클릭 시 아이템 아이콘이 마우스를 따라다니는 함수
    {
        if (selectedItem != null)
        {
            //Debug.Log($"{selectedItem.itemData.name} 드래그 중");
            rect.position = Input.mousePosition;
        }
    }

    private void RotateItem() // 아이템 회전하는 함수
    {
        if (selectedItem == null)
        {
            return;
        }

        selectedItem.Rotate();
    }

    private void HandleHighlight() // [인벤토리]에서 아이템 위에 마우스 올렸을때 형광색 하이라이트 표시하는 함수
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

    public void CreateItem(int id) // [주변]에서 아이템을 클릭했을 때 호출되는 함수
    {
        grabItem = true;

        InventoryItem_C inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem_C>();
        selectedItem = inventoryItem;

        itemID = id;

        rect = inventoryItem.GetComponent<RectTransform>();
        rect.SetParent(canvasT);
        rect.SetAsLastSibling();

        inventoryItem.Set(item[id - 100]);

        // ----- //

        InventoryControl.instance.RemoveItemFromNearList(id);
        UIManager.instance.CloseGetItemUI();
        isNewItem = true;
    }

    private void LeftMouseButtonPress() // 왼쪽 마우스 버튼 클릭 시 호출되는 함수
    {
        Vector2Int tileGridPosition = GetTileGridPosition();

        if(selectedItem != null)
        {
            if (selectedItemGrid.gridNumber == 0) // 인벤토리에 놓을 때
            {
                PlaceItem(tileGridPosition);
            }
            else if (selectedItemGrid.gridNumber == 1)
            {
                if (selectedItem.itemData.itemType == ItemData_C.ItemType.head)
                {
                    InventoryControl.instance.EquipItem(itemID);
                    PlaceItem(tileGridPosition);
                }
            }
            else if (selectedItemGrid.gridNumber == 2)
            {
                if (selectedItem.itemData.itemType == ItemData_C.ItemType.armor)
                {
                    InventoryControl.instance.EquipItem(itemID);
                    PlaceItem(tileGridPosition);
                }
            }
            else if (selectedItemGrid.gridNumber == 3)
            {
                if (selectedItem.itemData.itemType == ItemData_C.ItemType.weapon)
                {
                    InventoryControl.instance.EquipItem(itemID);
                    PlaceItem(tileGridPosition);
                }
            }
        }
        else
        {
            PickUpItem(tileGridPosition);
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

    private void PlaceItem(Vector2Int tileGridPosition) // 아이템 내려놓기
    {
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);

        if (complete)
        {
            if (overlapItem != null) // 아이템을 내려놓을 곳에 이미 아이템이 들어가 있을 때
            {
                if (selectedItemGrid.gridNumber == 3)
                {
                    InventoryControl.instance.UnEquipItem(overlapItem.itemData.itemID);
                    InventoryControl.instance.EquipItem(itemID);
                }

                DeleteItem(overlapItem); // 밑에 있던 아이템 빼고

                InsertItem(tileGridPosition.x, tileGridPosition.y, itemID); // 들고있던 아이템 넣고

                selectedItem = overlapItem; // 아이템 변경하고
                currentWidth = selectedItem.itemData.width; // width 바꿔주고
                currentHeight = selectedItem.itemData.height; // height 바꿔주고
                itemID = selectedItem.itemData.itemID; // itemID 바꿔주고

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

    private void PickUpItem(Vector2Int tileGridPosition) // [인벤토리]에서 아이템 클릭하여 픽업할 때 호출되는 함수
    {
        isNewItem = false;

        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);

        if (selectedItem == null)
        {
            return;
        }

        itemID = selectedItem.dataid;

        currentWidth = selectedItem.itemData.width;
        currentHeight = selectedItem.itemData.height;

        if (selectedItem != null)
        {
            rect = selectedItem.GetComponent<RectTransform>();
        }

        if (selectedItemGrid.gridNumber == 3)
        {
            InventoryControl.instance.UnEquipItem(itemID);
        }
        
        DeleteItem(selectedItem);
    }

    // ----------------------------------------------------------------------------------

    public void GridCheck() // 그리드(2차원 배열) 아이템이 들어있는 위치 출력
    {
        int count = 0;

        // 가로 17, 세로 28
        for (int x= 0; x < 17; x++)
        {
            for (int y = 0; y < 28; y++)
            {
                if (array[x, y] != 0)
                {
                    count += 1;
                    //Debug.Log($"확인 : {x},{y} = {array[x, y]}");
                }
            }
        }

        //Debug.Log("디아블로 인벤토리 : " + count);
    }

    public void InsertItem(int x, int y, int itemID) //아이템 롤백을 InsertItem과 DeleteItem 함수로 나눔
    {
        if (selectedItem == null)
        {
            Debug.Log("SelectedItem이 Null이다");
        }
        /*else
        {
            Debug.Log($"{selectedItem.itemData.itemName}을 array에 넣습니다");
            Debug.Log($"x, y : {x}, {y}");
            Debug.Log("itemID : " + itemID);
        }*/

        for (int i = 0; i < currentWidth; i++)
        {
            for (int j = 0; j < currentHeight; j++)
            {
                array[x + i, y + j] = itemID;
            }
        }

        if (isNewItem)
        {
            InventoryControl.instance.GetItem(itemID, 1);
        }

        GridCheck();
    }

    public void DeleteItem(InventoryItem_C item)
    {
        //Debug.Log($"{item.itemData.itemName}을 array에서 제거합니다");

        // 넣기는 되는데 왜 제대로 안빼졌나?
        // (클릭한 위치부터 아이템 모양만큼 빠지기 때문)
        // 그래서 아이템을 뺄 때에는 클릭 위치가 아닌 아이템의 왼쪽 상단 좌표부터 시작해야 한다. (selectedItem.onGridPosition 참조)

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
