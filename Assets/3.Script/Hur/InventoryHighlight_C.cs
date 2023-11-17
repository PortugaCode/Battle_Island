using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight_C : MonoBehaviour
{
    [SerializeField] RectTransform hightlighter;
    [SerializeField] private GridManager gm;

    [SerializeField] List<ItemData_C> itemdata;

    public bool sinho = false;
    public int itemID;
    public int _width;
    public int _height;

    private void Start()
    {
        gm = FindObjectOfType<GridManager>();
        //Caculator(0, 0);
    }
    private void Update()
    {
        DataLibrary();
    }
    public void DataLibrary()
    {
        switch (itemID)
        {
            case (100):
                (_width, _height) = (3, 1);
                break;
            case (101):
                (_width, _height) = (3, 4);
                break;
            case (102):
                (_width, _height) = (3, 4);
                break;
            case (103):
                (_width, _height) = (4, 4);
                break;
            case (104):
                (_width, _height) = (3, 4);
                break;
            case (105):
                (_width, _height) = (3, 4);
                break;
            case (106):
                (_width, _height) = (4, 4);
                break;
            case (107):
                (_width, _height) = (2, 2);
                break;
            case (108):
                (_width, _height) = (3, 2);
                break;
            case (109):
                (_width, _height) = (3, 3);
                break;
            case (110):
                (_width, _height) = (3, 3);
                break;
            case (111):
                (_width, _height) = (3, 3);
                break;
            case (112):
                (_width, _height) = (3, 3);
                break;
            case (113):
                (_width, _height) = (1, 2);
                break;
            case (114):
                (_width, _height) = (2, 2);
                break;
            case (115):
                (_width, _height) = (2, 2);
                break;
            case (116):
                (_width, _height) = (9, 3);
                break;
            case (117):
                (_width, _height) = (9, 3);
                break;
            default:
                Debug.Log("����Ʈ��");
                break;
        }
    }

    public void Show(bool b)
    {
        hightlighter.gameObject.SetActive(b);
    }

    public void SetSize(InventoryItem_C targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.WIDTH * ItemGrid_C.tileSizeWidth;
        size.y = targetItem.HEIGHT * ItemGrid_C.tileSizeHeight;

        hightlighter.sizeDelta = size;
    }

    public void SetPosition(ItemGrid_C targetGrid, InventoryItem_C targetItem)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGRid(
            targetItem,
            targetItem.onGridPositionX,
            targetItem.onGridPositionY);

        int width = gm.width;
        int height = gm.height;

        if (!sinho)
        {
            Debug.Log("���̶���Ʈ ON"); //�κ��丮 grid�� ���� ������ �� (���� �����۵� ��������)
            Debug.Log("������ǥ : " +
                (targetItem.onGridPositionX, targetItem.onGridPositionY));

            //���� ��ǥ
            int x = targetItem.onGridPositionX;
            int y = targetItem.onGridPositionY;

            //���
            Caculator(x,y);
            sinho = true;
        }

        hightlighter.localPosition = pos;
    }

    private void Caculator(int x, int y)
    {  //111111



        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                gm.array[x + i, y + j] = 1;

                Debug.Log($"gm.array(1) : {x + i},{y + j}");

            }
        }
        gm.AllGridCheck();

    }
    public void SetParent(ItemGrid_C targetGrid)
    {
        if (targetGrid == null)
        {
            return;
        }

        hightlighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }

    public void SetPosition(ItemGrid_C targetGrid, InventoryItem_C targetItem,
        int posX, int posY)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGRid(targetItem, posX, posY);

        hightlighter.localPosition = pos;
    }
}
