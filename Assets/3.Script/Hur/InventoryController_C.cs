using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController_C : MonoBehaviour
{
    [SerializeField] private ItemGrid_C selectedItemGrid;
    [SerializeField] private NoticeItem noticeItem;

    //주변에서 끌어오는 걸 구현할 경우 CreateRandomItem을 없애야 함

    InventoryItem_C selectedItem;
    InventoryItem_C overlapItem;
    RectTransform rect;

    [SerializeField] List<ItemData_C> item;
    [SerializeField] GameObject itemPrefab;
    
    [SerializeField] Transform canvasT;

    private InventoryHighlight_C inventoryHighlight;

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
    }
    private void Update()
    {
        ItemIconDrag();

        if (noticeItem.ItemDragOn) //수정 - 생략
        {
            if (selectedItem == null)
            {
                noticeItem.ItemDragOn = false;
                CreateRandomItem();

            }
        }

        //if (Input.GetKeyDown(KeyCode.Q)) //수정 - 생략
        //{
        //    if (selectedItem == null)
        //    {
        //        CreateRandomItem();
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.W)) //수정 - 생략
        //{
        //    InsertRandomItem();
        //}

        if (Input.GetMouseButtonDown(1))
        {
            RotateItem();
        }


        if (selectedItemGrid == null) //수정?
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

    //private void InsertRandomItem() //수정 - 생략
    //{
    //    if (selectedItemGrid == null)
    //    {
    //        return;
    //    }

    //    CreateRandomItem();
    //    InventoryItem_C itemToInsert = selectedItem;
    //    selectedItem = null;
    //    InsertItem(itemToInsert);
    //}

    //private void InsertItem(InventoryItem_C itemToInsert)//수정 - 생략
    //{
    //    Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);

    //    if (posOnGrid == null)
    //    {
    //        return;
    //    }

    //    selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    //}

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


    public void CreateRandomItem()//수정 - 생략
     //걍 애초에 slot프리팹에 이 함수를 넣으면 되잖아
    {
        Debug.Log("아이템 감지 in InventoryController");
        
        InventoryItem_C inventoryItem =
        Instantiate(itemPrefab).GetComponent<InventoryItem_C>();
        selectedItem = inventoryItem;

        rect = inventoryItem.GetComponent<RectTransform>();
        rect.SetParent(canvasT);
        rect.SetAsLastSibling();

        int selectedItemID = UnityEngine.Random.Range(0, item.Count); //랜덤 수정 - 변경
        inventoryItem.Set(item[selectedItemID]);

    }

    //public void CreateItem_wear()
    //{
    //    InventoryItem_C inventoryItem_wear =
    //    Instantiate(itemPrefab_wear).GetComponent<InventoryItem_C>();
    //    selectedItem = inventoryItem_wear;

    //    rect = inventoryItem_wear.GetComponent<RectTransform>();
    //    rect.SetParent(canvasT);
    //    rect.SetAsLastSibling();

    //    int selectedItemID_wear = UnityEngine.Random.Range(0, item.Count); //랜덤 수정 - 변경
    //    inventoryItem_wear.Set(item[selectedItemID_wear]);
    //}
    //public void CreateItem_etc()
    //{
    //    InventoryItem_C inventoryItem_etc =
    //       Instantiate(itemPrefab_etc).GetComponent<InventoryItem_C>();
    //    selectedItem = inventoryItem_etc;

    //    rect = inventoryItem_etc.GetComponent<RectTransform>();
    //    rect.SetParent(canvasT);
    //    rect.SetAsLastSibling();

    //    int selectedItemID_etc = UnityEngine.Random.Range(0, item.Count); //랜덤 수정 - 변경
    //    inventoryItem_etc.Set(item[selectedItemID_etc]);
    //}

    private void LeftMouseButtonPress()//수정 - 변경
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
