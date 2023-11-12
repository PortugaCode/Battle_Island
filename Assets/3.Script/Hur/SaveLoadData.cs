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
                    // 최근에 먹은 아이템을 리스트에 추가
                    ItemData recentItem = itemDataList[itemDataList.Count - 1];

                    // PlayerPrefs에 리스트를 JSON 문자열로 저장
                    string jsonData = JsonUtility.ToJson(itemDataList);
                    PlayerPrefs.SetString("ItemDataList", jsonData);
                    PlayerPrefs.Save();

                    Debug.Log($"Diablo에게 전할 것 : {recentItem.itemID}");
                    Debug.Log($"이것은 {recentItem.itemName}이다.");
                }
                else
                {
                    Debug.LogWarning("아이템 데이터를 찾을 수 없습니다!");
                }
            }
        }

        // PlayerPrefs에서 데이터 불러오기
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
