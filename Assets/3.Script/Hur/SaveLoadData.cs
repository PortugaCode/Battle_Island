using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadData : MonoBehaviour
{
    public class ItemData
    {
        public int itemID;
        public string itemName;
        public int itemCount;
    }
    public class ItemManager : MonoBehaviour
    {
        public List<ItemData> itemDataList = new List<ItemData>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (itemDataList.Count > 0)
                {
                    // �ֱٿ� ���� �������� ����Ʈ�� �߰�
                    ItemData recentItem = itemDataList[itemDataList.Count - 1];

                    // PlayerPrefs�� ����Ʈ�� JSON ���ڿ��� ����
                    string jsonData = JsonUtility.ToJson(itemDataList);
                    PlayerPrefs.SetString("ItemDataList", jsonData);
                    PlayerPrefs.Save();

                    Debug.Log($"Diablo���� ���� �� : {recentItem.itemID}");
                    Debug.Log($"�̰��� {recentItem.itemName}�̴�.");
                }
                else
                {
                    Debug.LogWarning("������ �����͸� ã�� �� �����ϴ�!");
                }
            }
        }

        // PlayerPrefs���� ������ �ҷ�����
        private void LoadItemData()
        {
            if (PlayerPrefs.HasKey("ItemDataList"))
            {
                string jsonData = PlayerPrefs.GetString("ItemDataList");
                itemDataList = JsonUtility.FromJson<List<ItemData>>(jsonData);
            }
        }

        private void Start()
        {
            LoadItemData();
        }

    }
}
