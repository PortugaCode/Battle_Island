using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_Grid : MonoBehaviour
{
    [SerializeField] private GameObject UIprefab; // UI 요소 프리팹
    [SerializeField] private RectTransform canvas; // Canvas 참조
    [SerializeField] private Text gridPos; //좌표
    
    public int[,] array = new int[17, 28]; // 2차원 배열
    public float tileSize = 32f; // UI 요소의 크기

    private void Start()
    {
        CreateUI();
        //Find_itemGrid(UIprefab);
        gridPos = GameObject.Find("Text").GetComponent<Text>();
    }
    private void CreateUI() //ui인벤토리 배열
    {
        // 배열을 순회하며 각 요소에 따라 UI 요소 생성 및 위치 설정
        for (int x = 0; x < array.GetLength(0); x++)
        {
            for (int y = 0; y < array.GetLength(1); y++)
            {
                // UI 요소 생성
                GameObject ui = Instantiate(UIprefab, canvas);

                // UI 요소의 RectTransform 컴포넌트 가져오기
                RectTransform rectTransform = ui.GetComponent<RectTransform>();

                //Text
                gridPos.text = " " + new Vector2(x, y);

                // UI 요소의 위치 설정
                rectTransform.anchoredPosition =
                    new Vector2(x * tileSize, -y * tileSize);
                // 여기서 y의 음수는 Canvas의 좌표 체계에 맞춰서 아래로 내려가게 함

                Image image = ui.GetComponent<Image>();

            }
        }
    }
    //private void Find_itemGrid(GameObject ui)
    //{
    //    //if (inv_highlight.sinho)
    //    //{
    //    //    inv_highlight.sinho = false;
    //    //}

    //    //int width = targetItem.onGridPositionX;
    //    //int height = targetItem.onGridPositionY;

    //    //one_two(3x2)인 경우
    //    for (int width = 0; width < 3; width++)
    //    {
    //        for (int height = 0; height < 2; height++)
    //        {
    //            Caculator(array, width, height, Image image);

    //            Debug.Log("w/h : " + (width, height));
    //        }
    //    }
    //}
    private void Caculator(int[,] array, int width, int height, Image image)
    {
        //3x2 형태
        if (width == 0 && height == 0)//시작 좌표값
        {
            //인덱스값 -> 1을 itemID로 바꿔야함!!
            array[width, height] = 1;
            array[width + 1, height] = 1;
            array[width + 2, height] = 1;
            array[width, height + 1] = 1;
            array[width + 1, height + 1] = 1;
            array[width + 2, height + 1] = 1;

            Color color = (array[width, height] == 0) ? Color.white : Color.red;
            image.color = color;
        }
    }
}
