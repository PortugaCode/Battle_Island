using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight_C : MonoBehaviour
{
    [SerializeField] RectTransform hightlighter;
    [SerializeField] private GridManager gm;
    [SerializeField] private InventoryController_C inv_cont;
    [SerializeField] List<ItemData_C> itemdata;
    [SerializeField] private OpenInvent openIvn;

    public bool sinho = false;
    public int itemID;
    public int _width;
    public int _height;

    private void Start()
    {
        gm = FindObjectOfType<GridManager>();
        inv_cont = FindObjectOfType<InventoryController_C>();
        openIvn = FindObjectOfType<OpenInvent>();
        //Caculator(0, 0);

        //if (itemdata != null && itemdata.Count > 0)
        //{
        //    for (int i = 0; i < itemdata.Count; i++)
        //    {
        //        ItemData_C pickItem = itemdata[i]; // ����Ʈ�� n��° ��� ��������

        //        _width = pickItem.width;
        //        _height = pickItem.height;
        //        itemID = pickItem.itemID;
        //    }

        //}
        //else
        //{
        //    return;
        //}

    }
    private void Update()
    {
        //if (openIvn.openThisSC)
        //{
        //    DataLibrary(itemID);
        //}
        //DataLibrary(itemID);

    }
    public void DataLibrary(int itemID)
    {
        openIvn.openThisSC = false;

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

    public void SetPosition(ItemGrid_C targetGrid, InventoryItem_C targetItem, int itemID)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGRid(
            targetItem,
            targetItem.onGridPositionX,
            targetItem.onGridPositionY);

        int width = gm.width;
        int height = gm.height;

        if (!sinho)
        {
            //Debug.Log("���̶���Ʈ ON"); //�κ��丮 grid�� ���� ������ �� (���� �����۵� ��������)
            //Debug.Log("������ǥ : " +
            //    (targetItem.onGridPositionX, targetItem.onGridPositionY));

            //���� ��ǥ
            int x = targetItem.onGridPositionX;
            int y = targetItem.onGridPositionY;

            //���
            Caculator(x, y, itemID);
            sinho = true;
        }

        hightlighter.localPosition = pos;
    }

    private void Caculator(int x, int y, int itemID)
    {
        //Debug.Log(inv_cont.rollback);

        if (!inv_cont.rollback)
        {
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    gm.array[x + i, y + j] = itemID; // 1 -> ������ ID�� �ٲ����

                    //Debug.Log($"gm.array : {x + i},{y + j}");

                }
            }
            inv_cont.rollback = true;
        }
        else //�ѹ��̸�
        {
            // array[17,28] �̴ϱ�
            // x+i �� 16 ���Ͽ����ϰ�
            // y+j �� 27���Ͽ����Ѵ�

            // x �� �������� �ʺ�?
            // y �� �������� ����

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    gm.array[i + x, j + y] = 0;

                    //Debug.Log($"�ѹ�array : {x + i},{y + j}");

                }
            }
            inv_cont.rollback = false;
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
