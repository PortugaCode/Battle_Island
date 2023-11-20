using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem_C : MonoBehaviour
{
    //�� ��ũ��Ʈ�� Diablo�� �κ��丮�� �� �������� ũ�⸦ ������

    public ItemData_C itemData;
    public int checkpoint;

    [SerializeField] private InventoryHighlight_C inventoryHighlight_C;
    public int dataid;

    public int HEIGHT
    {
        get
        {
            if (!rotated)//ȸ���� �ƴ϶�� ���̴� ����
            {
                return itemData.height;
            }
            return itemData.width;//ȸ���� �ߴٸ� ���� -> ���̷� ��ȯ
        }
    }
    public int WIDTH
    {
        get
        {
            if (!rotated)
            {
                return itemData.width;
            }
            return itemData.height;
        }
    }

    public int onGridPositionX;
    public int onGridPositionY;

    public bool rotated = false;
    public bool delay = false;

    private void Awake()
    {
        //inventoryHighlight_C = FindObjectOfType<InventoryHighlight_C>();
    }
    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    //Debug.Log("Ŭ��");
    //    inventoryHighlight_C.itemID = dataid;
    //}

    public void Set(ItemData_C itemData)//������ ���� �������� ��
    {
        this.itemData = itemData;
        dataid = itemData.itemID;
        GetComponent<Image>().sprite = itemData.itemIcon;

        Vector2 size = new Vector2();
        size.x = itemData.width * ItemGrid_C.tileSizeWidth;
        size.y = itemData.height * ItemGrid_C.tileSizeHeight;
        GetComponent<RectTransform>().sizeDelta = size;
    }

    public void Rotate()
    {
        rotated = !rotated;
        RectTransform rect = GetComponent<RectTransform>();
        rect.rotation = Quaternion.Euler(0, 0, rotated == true ? 90f : 0f);
    }


}

