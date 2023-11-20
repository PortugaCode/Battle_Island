using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_Grid : MonoBehaviour
{
    [SerializeField] private GameObject UIprefab; // UI ��� ������
    [SerializeField] private RectTransform canvas; // Canvas ����
    [SerializeField] private Text gridPos; //��ǥ
    
    public int[,] array = new int[17, 28]; // 2���� �迭
    public float tileSize = 32f; // UI ����� ũ��

    private void Start()
    {
        CreateUI();
        //Find_itemGrid(UIprefab);
        gridPos = GameObject.Find("Text").GetComponent<Text>();
    }
    private void CreateUI() //ui�κ��丮 �迭
    {
        // �迭�� ��ȸ�ϸ� �� ��ҿ� ���� UI ��� ���� �� ��ġ ����
        for (int x = 0; x < array.GetLength(0); x++)
        {
            for (int y = 0; y < array.GetLength(1); y++)
            {
                // UI ��� ����
                GameObject ui = Instantiate(UIprefab, canvas);

                // UI ����� RectTransform ������Ʈ ��������
                RectTransform rectTransform = ui.GetComponent<RectTransform>();

                //Text
                gridPos.text = " " + new Vector2(x, y);

                // UI ����� ��ġ ����
                rectTransform.anchoredPosition =
                    new Vector2(x * tileSize, -y * tileSize);
                // ���⼭ y�� ������ Canvas�� ��ǥ ü�迡 ���缭 �Ʒ��� �������� ��

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

    //    //one_two(3x2)�� ���
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
        //3x2 ����
        if (width == 0 && height == 0)//���� ��ǥ��
        {
            //�ε����� -> 1�� itemID�� �ٲ����!!
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
