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
    public bool rollback = false; // 1 -> 0으로 돌아가기

    public int itemID;
    private Slot slot;

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
        slot = GetComponent<Slot>();

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

            if (item != null)
            {

                for (int i = 0; i < item.Count; i++)
                {
                    //ItemData_C pickItem = item[i]; // 리스트의 n번째 ID 가져오기

                    switch (i)
                    {
                        case 0:
                            itemID = 100;
                            break;
                        case 1:
                            itemID = 101;
                            break;
                        case 2:
                            itemID = 102;
                            break;
                        case 3:
                            itemID = 103;
                            break;
                        case 4:
                            itemID = 104;
                            break;
                        case 5:
                            itemID = 105;
                            break;
                        case 6:
                            itemID = 106;
                            break;
                        case 7:
                            itemID = 107;
                            break;
                        case 8:
                            itemID = 108;
                            break;
                        case 9:
                            itemID = 109;
                            break;
                        case 10:
                            itemID = 110;
                            break;
                        case 11:
                            itemID = 111;
                            break;
                        case 12:
                            itemID = 112;
                            break;
                        case 13:
                            itemID = 113;
                            break;
                        case 14:
                            itemID = 114;
                            break;
                        case 15:
                            itemID = 115;
                            break;
                        case 16:
                            itemID = 116;
                            break;
                        case 17:
                            itemID = 117;
                            break;

                    }

                }
            }

            if (itemToHighlight != null)
            {
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighlight);
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight, itemID);

                gridmanager.Find_itmeGrid(0, 0);
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

    public void CreateItem()
    {
        //Debug.Log("아이템 감지 in InventoryController");

        InventoryItem_C inventoryItem =
        Instantiate(itemPrefab).GetComponent<InventoryItem_C>();
        selectedItem = inventoryItem;

        rect = inventoryItem.GetComponent<RectTransform>();
        rect.SetParent(canvasT);
        rect.SetAsLastSibling();

        int selectedItemID = 0;

        switch (matchingID)
        {
            case (100):
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
            default:
                Debug.Log("디폴트값");
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

        inventoryHighlight.itemID = selectedItem.gameObject.GetComponent<InventoryItem_C>().dataid; //클릭될 때 정보 교류

        //픽업을 할 때 0으로 바꿔버리기 
        rollback = true;

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
