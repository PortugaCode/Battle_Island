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
    [SerializeField] private OpenInvent openInv;
    
    public int[,] array = new int[17, 28];
    private void Start()
    {
        inv_highlight = FindObjectOfType<InventoryHighlight_C>();
        inv_item = FindObjectOfType<InventoryItem_C>();
        openInv = FindObjectOfType<OpenInvent>();
    }
    private void Update()
    {
        Find_itmeGrid(inv_item);

        if (openInv.Open)
        {
            Check_grid(true);
        }
    }
    private void Find_itmeGrid(InventoryItem_C targetItem)
    {
        //inv_item�� �� �ƴ� ��
        if (inv_highlight.sinho)
        {
            inv_highlight.sinho = false;

            //int width = targetItem.onGridPositionX;
            //int height = targetItem.onGridPositionY;

            //one_two(3x2)�� ���
            for (int width = 0; width < 3; width++)
            {
                for (int height = 0; height < 2; height++)
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
    private void Check_grid(bool check) //üũ�ϱ� ���� �Լ�(���� - ���߿� �����)
    {
        check_value.gameObject.SetActive(check);
    }
}

