using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeItem : MonoBehaviour
{
    [SerializeField] private Database_hur database;
    [SerializeField] private GameObject slotprefab;
    [SerializeField] private Transform parentTransform;

    private int num;
    private string nameSave;
    //public Text itemCount_txt;
    //public GameObject selectedItem;
    public int addItem = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ItemData_hur randomItem = GetRandomItem();
            GameObject newPrefab = Instantiate(slotprefab, parentTransform);
            UpdateSlot(newPrefab, randomItem);
        }
    }

    private ItemData_hur GetRandomItem()
    {
        int randomindex = Random.Range(0, database.itemList.Count);
        return database.itemList[randomindex];
    }
    private void UpdateSlot(GameObject prefab, ItemData_hur itemdata)
    {
        Text itemNameText = prefab.GetComponentInChildren<Text>();
        Image itemImage = prefab.GetComponentInChildren<Image>();

        if (itemNameText != null)
        {
            itemNameText.text = itemdata.itemName;
        }

        if (itemImage != null)
        {
            itemImage.sprite = itemdata.itemIcon;
        }
    }
}
