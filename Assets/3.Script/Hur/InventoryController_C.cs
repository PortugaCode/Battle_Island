using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController_C : MonoBehaviour
{
    [SerializeField] private ItemGrid_C selectedItemGrid;
    [SerializeField] private NoticeItem noticeItem;

    InventoryItem_C selectedItem;
    InventoryItem_C overlapItem;
    RectTransform rect;

    [SerializeField] List<ItemData_C> item;
    [SerializeField] GameObject itemPrefab;
    
    [SerializeField] Transform canvasT;
    [SerializeField] GridManager gridmanager;

    private InventoryHighlight_C inventoryHighlight;

    public int matchingID;

    //주변 슬롯
    public Slot[] slots;
    public Transform slotHolder;

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
        noticeItem = FindObjectOfType<NoticeItem>();
        slots = slotHolder.GetComponentsInChildren<Slot>();
        gridmanager = FindObjectOfType<GridManager>();
    }
    private void Update()
    {
        ItemIconDrag();

        //확인용 - Grid, 좌표
        if (Input.GetMouseButton(0))
        {
            //Debug.Log(selectedItemGrid.GetTileGridPosition(Input.mousePosition));
        }

        if (noticeItem.ItemDragOn)
        {
            if (selectedItem == null)
            {
                noticeItem.ItemDragOn = false;
                CreateItem();

            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RotateItem();
        }


        if (selectedItemGrid == null)
        {
            inventoryHighlight.Show(false);
            return;
        }

        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }
    }

    private void RotateItem()
    {
        if (selectedItem == null)
        {
            return;
        }

        selectedItem.Rotate();
    }

    Vector2Int oldPosition;
    InventoryItem_C itemToHighlight;

    private void HandleHighlight()
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
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);

                gridmanager.Find_itmeGrid(selectedItemGrid, itemToHighlight);
            }
            else
            {
                inventoryHighlight.Show(false);
            }
        }
        else
        {
            inventoryHighlight.Show(selectedItemGrid.BoundryCheck(
                positionOnGrid.x,
                positionOnGrid.y,
                selectedItem.WIDTH,
                selectedItem.HEIGHT)
                );
            inventoryHighlight.SetSize(selectedItem);
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem,
                positionOnGrid.x, positionOnGrid.y);
        }

    }

    public void CreateItem()//수정 - 생략 //슬롯 스크립트랑 연결할것
     //slot프리팹에 넣을 함수
    {
        //Debug.Log("아이템 감지 in InventoryController");
        
        InventoryItem_C inventoryItem = 
        Instantiate(itemPrefab).GetComponent<InventoryItem_C>();
        selectedItem = inventoryItem;

        rect = inventoryItem.GetComponent<RectTransform>();
        rect.SetParent(canvasT);
        rect.SetAsLastSibling();

        //int matchingID = PlayerPrefs.GetInt("아이템 아이디"); //아이템 정보 - 슬롯 몇번째를 눌렀는지 정보를 알아내면 됨 //아이템아이디

        //여러 개 같이 묶여서 덮어쓰는 중. 따로 눌러서 할 수 있음?

        int selectedItemID = 0;

        switch (matchingID)
        {
            case(100): 
                selectedItemID = 0;
                break;
            case (101): 
                selectedItemID = 1;
                break;
            case (102):
                selectedItemID = 2;
                break;
            case (103):
                selectedItemID = 3;
                break;
            case (104):
                selectedItemID = 4;
                break;
            case (105):
                selectedItemID = 5;
                break;
            case (106):
                selectedItemID = 6;
                break;
            case (107):
                selectedItemID = 7;
                break;
            case (108):
                selectedItemID = 8;
                break;
            case (109):
                selectedItemID = 9;
                break;
            case (110):
                selectedItemID = 10;
                break;
            case (111):
                selectedItemID = 11;
                break;
            case (112):
                selectedItemID = 12;
                break;
            case (113):
                selectedItemID = 13;
                break;
            case (114):
                selectedItemID = 14;
                break;
            case (115):
                selectedItemID = 15;
                break;
            case (116):
                selectedItemID = 16;
                break;
            case (117):
                selectedItemID = 17;
                break;
            default: Debug.Log("디폴트값");
                break;
        }

        inventoryItem.Set(item[selectedItemID]);
    }

    private void LeftMouseButtonPress()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();

        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
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

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        bool complete =
            selectedItemGrid.PlaceItem
            (selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);

        if (complete)
        {
            selectedItem = null;

            if (overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rect = selectedItem.GetComponent<RectTransform>();
                rect.SetAsLastSibling();
            }
        }
    }
    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = 
            selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);

        if (selectedItem != null)
        {
            rect = selectedItem.GetComponent<RectTransform>();
        }
    }
    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            rect.position = Input.mousePosition;
        }
    }
    
}
