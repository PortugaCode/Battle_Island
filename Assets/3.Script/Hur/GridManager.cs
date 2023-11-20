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
    [SerializeField] private RectTransform check_value;
    [SerializeField] private OpenInvent openIvn;
    public ItemData_C itemData;

    public int[,] array = new int[17, 28]; // �κ��丮 width, height ���� ���� new int[width, height] (�����Ҵ�)

    //������ ����, ����
    public int width;
    public int height;

    private void Start()
    {
        inv_highlight = FindObjectOfType<InventoryHighlight_C>();
        openIvn = FindObjectOfType<OpenInvent>();

        if (openIvn.openThisSC)
        {
            AllGridZero();
        }
       
    }

    private void AllGridZero()
    {
        openIvn.openThisSC = false;

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
    public void Find_itmeGrid(int width, int height) //�̰� �ٽ� �����Ұ� ������ Ȯ��
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

