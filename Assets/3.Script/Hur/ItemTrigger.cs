 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class ItemTrigger : MonoBehaviour
{
    public ItemData_hur itemData; // �ν��Ͻ��� Inspector���� �Ҵ�
    public ItemData_C itemData_c; //Diablo �κ��丮���� ������ ��� ��
    //[SerializeField] List<ItemData_C> itemData_C; //Diablo �κ��丮���� ������ ��� ��

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (itemData != null)
            {
                //Debug.Log($"�̸�: {itemData.itemName}, ���̵�: {itemData.itemID}, ����: {itemData.itemCount}");

                PlayerPrefs.SetInt("������ ID", itemData.itemID);
                PlayerPrefs.SetString("������ �̸�", itemData.itemName);
                PlayerPrefs.SetInt("������ ����", itemData.itemCount);

                //Diablo �κ��丮���� ������ ��� ��
                PlayerPrefs.SetInt("Item_ID", itemData_c.itemID);
                Debug.Log($"Diblo���� ���� �� : {PlayerPrefs. GetInt("Item_ID")}");

                Debug.Log($"2. �̰��� {PlayerPrefs.GetString("������ �̸�")}�̴�.");
            }
            else
            {
                Debug.LogWarning("������ �����͸� ã�� �� �����ϴ�!");
            }
        }
    }
    public void RememberInfo()
    {
        
    }
}
