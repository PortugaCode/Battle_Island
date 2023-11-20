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
    [SerializeField] private RectTransform check_value;
    [SerializeField] private OpenInvent openIvn;
    public ItemData_C itemData;

    public int[,] array = new int[17, 28]; // 인벤토리 width, height 따로 만들어서 new int[width, height] (동적할당)

    //아이템 넓이, 높이
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
                Debug.Log($"확인 : {i},{j} = {array[i, j]}");
            }
        }

    }

    private void Update()
    {
        Find_itmeGrid(width, height);
    }
    public void Find_itmeGrid(int width, int height) //이거 다시 수정할거 없는지 확인
    {
        //inv_item일 때 아닐 때
        if (inv_highlight.sinho)
        {
            inv_highlight.sinho = false;

            for (width = 0; width < 3; width++)
            {
                for (height = 0; height < 2; height++)
                {
                    Debug.Log("부피 : " + (width, height)); //부피
                }
            }
        }
    }
}

