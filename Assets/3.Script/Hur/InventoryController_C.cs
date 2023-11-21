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

    [SerializeField] GameObject tooltipBtn;

    private InventoryHighlight_C inventoryHighlight;

    public bool grabItem = false; // 현재 마우스에 아이템을 들고있는가?
    public bool isOverlap = false; // 인벤토리에 아이템을 놓을 때 겹치는가?

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

    // [다른 그리드]
    public bool isHeadEquip = false;
    public bool isArmorEquip = false;
    public bool isGunEquip = false;

    // 슬롯 선택
    public int currentWidth;
    public int currentHeight;

    //툴팁 버튼 카운트
    public bool tip = false;

    //아이템 오브젝트 담을 변수
    public GameObject item1;

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
    }
    private void Update()
    {
        if (selectedItemGrid == null)
        {
            inventoryHighlight.Show(false);
            return;
        }

        ItemIconDrag(); // [주변]에서 아이템 아이콘 클릭 시 아이템 아이콘이 마우스를 따라다니는 함수

        HandleHighlight(); // [인벤토리]에서 아이템 위에 마우스 올렸을때 형광색 하이라이트 표시하는 함수

      

        if (Input.GetMouseButtonDown(1)) // 마우스 우클릭
        {
            Vector2Int tileGridPosition = GetTileGridPosition();
            TooltipClick(tileGridPosition); //툴팁 메서드 (버리기, 사용)
            //TooltipClick(); //툴팁 메서드 (버리기, 사용)

        }
        if (Input.GetKeyDown(KeyCode.R))
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
    public void TooltipClick(Vector2Int tileGridPosition) 
        //public void TooltipClick()
        //로컬 포지션을 마우스 포인터에 연결 (0,0)
    {
        if (selectedItem == null) 
            //'아무것도 없는 인벤토리에서' 라는 조건 추가 
            // 좌클릭한 아이템이 어떤 아이템인지 인지
        {
            //selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
            
            if (selectedItem!=null)
            {
                Debug.Log(selectedItem.itemData.itemName);
            }

            if (!tip) //짝수일 때
            {
                tooltipBtn.SetActive(true);
                tip = true;
                Debug.Log("툴팁 ON");
            }
            else if(tip)
            {
                tooltipBtn.SetActive(false); 
                tip = false;
                Debug.Log("툴팁 OFF");
            }
        }
       
    }

    private void RotateItem() // 아이템 회전하는 함수
    {
        //if (selectedItem == null) // 만약 잡고 있는 아이템이 없다면
        //{
        //    return;
        //}

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
    }

    private void LeftMouseButtonPress() // 왼쪽 마우스 버튼 클릭 시 호출되는 함수
    {
        Vector2Int tileGridPosition = GetTileGridPosition();

        //---------------------

        //if (selectedItem == null) // 현재 선택한 아이템이 없으면 아이템 픽업
        //{
        //    PickUpItem(tileGridPosition);
        //}
        //else // 이미 선택한 아이템이 있으면 아이템 내려놓기
        //{
        //    PlaceItem(tileGridPosition);
        //}

        //----------------------

        if(selectedItem != null)
        {
            if (selectedItemGrid.gridNumber == 0)
            {
                PlaceItem(tileGridPosition);
            }
            else if (selectedItemGrid.gridNumber == 1)
            {
                if (selectedItem.itemData.itemType == ItemData_C.ItemType.head)
                {
                    PlaceItem(tileGridPosition);
                    Debug.Log("head");
                }
                else
                {
                    PickUpItem(tileGridPosition);
                }
            }
            else if (selectedItemGrid.gridNumber == 2)
            {
                if (selectedItem.itemData.itemType == ItemData_C.ItemType.armor)
                {
                    PlaceItem(tileGridPosition);
                    Debug.Log("armor");
                }
            }
            else if (selectedItemGrid.gridNumber == 3)
            {
                if (selectedItem.itemData.itemType == ItemData_C.ItemType.weapon)
                {
                    PlaceItem(tileGridPosition);
                    Debug.Log("weapon");
                }
            }
        }
        else
        {
            PickUpItem(tileGridPosition);
        }
<<<<<<< Updated upstream

=======
        else // 이미 선택한 아이템이 있으면 아이템 내려놓기
        {
            /*아이템 type과 grid type이 일치하면 PlaceItem, 아니면 return*/

            if (selectedItemGrid.GetComponent<ItemGrid_C>().gridNumber == 0)
            {
                PlaceItem(tileGridPosition);
            }
            else if (selectedItemGrid.GetComponent<ItemGrid_C>().gridNumber == 1)
            {
                if (selectedItem.itemData.itemType == ItemData_C.ItemType.head)
                {
                    PlaceItem(tileGridPosition);
                }
            }
            else if (selectedItemGrid.GetComponent<ItemGrid_C>().gridNumber == 2)
            {
                if (selectedItem.itemData.itemType == ItemData_C.ItemType.armor)
                {
                    PlaceItem(tileGridPosition);
                }
            }
            else if (selectedItemGrid.GetComponent<ItemGrid_C>().gridNumber == 3)
            {
                if (selectedItem.itemData.itemType == ItemData_C.ItemType.weapon)
                {
                    PlaceItem(tileGridPosition);
                }
            }
            else
            {
                Debug.Log("착용불가");
            }
            
        }
>>>>>>> Stashed changes
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
                //Debug.Log("Overlap 발생");

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
        //Debug.Log("[인벤토리]에서 아이템 픽업");

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

        DeleteItem(selectedItem);
        //Calculate(tileGridPosition.x, tileGridPosition.y, itemID);
    }

    // ----------------------------------------------------------------------------------

    public void GridCheck() // 그리드(2차원 배열) 아이템이 들어있는 위치 출력
    {
        int count = 0;

        // 가로 17, 세로 28
        for (int i = 0; i < 17; i++)
        {
            for (int j = 0; j < 28; j++)
            {
                if (array[i, j] != 0)
                {
                    count += 1;
                    Debug.Log($"확인 : {i},{j} = {array[i, j]}");
                }
            }
        }

        //Debug.Log("디아블로 인벤토리 : " + count);
    }

    /*public void Calculate(int x, int y, int itemID)
    {
        if (grabItem) // 아이템을 들고 있을 때 array에 itemID 넣기
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
        else // 아이템을 안 들고 있을 때 array에서 itemID 빼기
        {
            // 넣기는 되는데 왜 제대로 안빼졌나?
            // (클릭한 위치부터 아이템 모양만큼 빠지기 때문)
            // 그래서 아이템을 뺄 때에는 클릭 위치가 아닌 아이템의 왼쪽 상단 좌표부터 시작해야 한다. (selectedItem.onGridPosition 참조)
            
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

    public void InsertItem(int x, int y, int itemID) //아이템 롤백을 InsertItem과 DeleteItem 함수로 나눔
    {
        if (selectedItem == null)
        {
            //Debug.Log("SelectedItem이 Null이다");
        }
        else
        {
            //Debug.Log($"{selectedItem.itemData.itemName}을 array에 넣습니다");
        }

        if (selectedItemGrid.GetComponent<ItemGrid_C>().gridNumber == 0)
        {
            for (int i = 0; i < currentWidth; i++)
            {
                for (int j = 0; j < currentHeight; j++)
                {
                    array[x + i, y + j] = itemID;
                }
            }
        }

        if (selectedItemGrid.GetComponent<ItemGrid_C>().gridNumber == 1)
        {
            if (selectedItem.itemData.itemType == ItemData_C.ItemType.head)
            {
                isHeadEquip = true;
            }
        }
        else if (selectedItemGrid.GetComponent<ItemGrid_C>().gridNumber == 2)
        {
            if (selectedItem.itemData.itemType == ItemData_C.ItemType.armor)
            {
                isArmorEquip = true;
            }
        }
        else if (selectedItemGrid.GetComponent<ItemGrid_C>().gridNumber == 3)
        {
            if (selectedItem.itemData.itemType == ItemData_C.ItemType.weapon)
            {
                isGunEquip = true;
            }
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
