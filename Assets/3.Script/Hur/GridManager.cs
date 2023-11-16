using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    /*
     * 2차원 배열로 inventory 칸을 대입하고 칸이 차지가 안되면 0,
     * 차지되면 item ID 숫자 나오게 하기
     * 인벤토리 UI가 없어도 데이터가 있어서 표에 아이템이 차지한 넓이가 표시되게 하기
     */
    [SerializeField] private InventoryHighlight_C inv_highlight;
    [SerializeField] private InventoryItem_C inv_item;
    [SerializeField] private RectTransform check_value;
    [SerializeField] private ItemGrid_C itemgrid;

    public int[,] array = new int[17, 28];
    //public int width;
    //public int height;

    private void Start()
    {
        inv_highlight = FindObjectOfType<InventoryHighlight_C>();
        inv_item = FindObjectOfType<InventoryItem_C>();
        itemgrid = FindObjectOfType<ItemGrid_C>();
    }
    //private void Update()
    //{
    //   Find_itmeGrid();

    //}
    public void Find_itmeGrid(ItemGrid_C selectedItemGrid, 
        InventoryItem_C itemToHighlight)
    {
        Vector2 pos = selectedItemGrid.CalculatePositionOnGRid(
            itemToHighlight,
            itemToHighlight.onGridPositionX,
            itemToHighlight.onGridPositionY);

        int width = itemToHighlight.onGridPositionX;
        int height = itemToHighlight.onGridPositionY;

        //inv_item일 때 아닐 때
        if (inv_highlight.sinho)
        {
            inv_highlight.sinho = false;

            //one_two(3x2)인 경우
            for (width = 0; width < 3; width++)
            {
                for (height = 0; height < 2; height++)
                {
                    Caculator(array, width, height);

                    Debug.Log("w/h : " + (width, height));
                }
            }

        }
    }

    private void Caculator(int[,] array, int width, int height)
    {
        //3x2 형태
        //인덱스값 -> 1을 itemID로 바꿔야함!!
        array[width, height] = 1;
        array[width + 1, height] = 1;
        array[width + 2, height] = 1;
        array[width, height + 1] = 1;
        array[width + 1, height + 1] = 1;
        array[width + 2, height + 1] = 1;

    }

}

