using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem_C : MonoBehaviour
{
    //�� ��ũ��Ʈ�� Diablo�� �κ��丮�� �� �������� ũ�⸦ ������

    public ItemData_C itemData;

    public int HEIGHT
    {
        get
        {
            if (rotated == false)//ȸ���� �ƴ϶�� ���̴� ����
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
            if (rotated == false)
            {
                return itemData.width;
            }
            return itemData.height;
        }
    }

    public int onGridPositionX;
    public int onGridPositionY;

    public bool rotated = false;

    public void Set(ItemData_C itemData)
    {
        this.itemData = itemData;

        GetComponent<Image>().sprite = itemData.itemIcon;//�̰� ������ ������ itemIcon��? ���� C��

        //������ �ֺ� �κ��丮���� ����� �����۰� ���ƾ� ��
        Debug.Log("������ ��");
        Debug.Log($"Diablo�� ������ ���� : {PlayerPrefs.GetInt("Item_ID")}");

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
