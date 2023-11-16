using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    /*
     * 2���� �迭�� inventory ĭ�� �����ϰ� ĭ�� ������ �ȵǸ� 0,
     * �����Ǹ� item ID ���� ������ �ϱ�
     * �κ��丮 UI�� ��� �����Ͱ� �־ ǥ�� �������� ������ ���̰� ǥ�õǰ� �ϱ�
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

        //inv_item�� �� �ƴ� ��
        if (inv_highlight.sinho)
        {
            inv_highlight.sinho = false;

            //one_two(3x2)�� ���
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
        //3x2 ����
        //�ε����� -> 1�� itemID�� �ٲ����!!
        array[width, height] = 1;
        array[width + 1, height] = 1;
        array[width + 2, height] = 1;
        array[width, height + 1] = 1;
        array[width + 1, height + 1] = 1;
        array[width + 2, height + 1] = 1;

    }

}

