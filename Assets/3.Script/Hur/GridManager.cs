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
    public ItemData_C itemData;

    public int[,] array = new int[17, 28]; // �κ��丮 width, height ���� ���� new int[width, height] (�����Ҵ�)

    //������ ����, ����
    public int width; 
    public int height;

    private void Start()
    {
        inv_highlight = FindObjectOfType<InventoryHighlight_C>();
        inv_item = FindObjectOfType<InventoryItem_C>();
        itemgrid = FindObjectOfType<ItemGrid_C>();
        AllGridZero();
    }

    private void AllGridZero()
    {
        for (int i = 0; i < 17; i++)
        {
            for (int j = 0; j < 28; j++)
            {
                array[i, j] = 0;
            }
        }
        AllGridCheck();
    }

    public void AllGridCheck()
    {
        //�������� �ٸ� ������ �Ű��� ���� ���� 1 -> 0
        for (int i = 0; i < 17; i++)
        {
            for (int j = 0; j < 28; j++)
            {
                Debug.Log($"Ȯ�� : {i},{j} = {array[i, j]}");
            }
        }
    }

    private void Update()
    {
        Find_itmeGrid(width, height);
    }
    public void Find_itmeGrid(int width, int height)
    {
        //inv_item�� �� �ƴ� ��
        if (inv_highlight.sinho)
        {
            inv_highlight.sinho = false;

            for (width = 0; width < 3; width++)
            {
                for (height = 0; height < 2; height++)
                {
                    Debug.Log("���� : " + (width, height)); //����
                }
            }
        }
    }
}

