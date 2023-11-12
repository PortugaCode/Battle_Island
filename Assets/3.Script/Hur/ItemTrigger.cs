 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    public ItemData_hur itemData; // �ν��Ͻ��� Inspector���� �Ҵ�
    public ItemData_C itemData_c; //Diablo �κ��丮���� ������ ��� ��

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (itemData != null)
            {
                //�ֱٿ� ������ ���� �ϳ��� ��
                PlayerPrefs.SetInt("������ ID", itemData.itemID);
                PlayerPrefs.SetString("������ �̸�", itemData.itemName);
                PlayerPrefs.SetInt("������ ����", itemData.itemCount);

                //Diablo �κ��丮���� ������ ��� ��
                //�ֱٿ� ������ ���� �ϳ��� ��
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
}
