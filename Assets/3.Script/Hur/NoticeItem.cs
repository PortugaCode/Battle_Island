using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NoticeItem : MonoBehaviour
{
    [SerializeField] private Database_hur database;
    [SerializeField] private GameObject slotprefab;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private PlayerController player;

    public bool ItemDragOn = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        database = FindObjectOfType<Database_hur>();
    }
    private void Update()
    {
        if (player.eatItem)
        {   
            KnowItem();
        }
        
    }

    private void KnowItem()
    //�������� �������� �� ���� ������ ����
    {
        player.eatItem = false;

        //���� �����ߴ� ������ �����Ͱ� Surround �κ��丮�� ��ٸ�
        //Debug.Log("3. ������ ���� in NoticeItem");

        ItemData_hur itemData = MatchInfo();
        GameObject SurSlotPrefab = Instantiate(slotprefab, parentTransform);
        UpdateSlot(SurSlotPrefab, itemData);
    }

    private ItemData_hur MatchInfo()
    //itemList�� �ش��ϴ� �����۰� �̾����� ��
    {
        int Itemindex = PlayerPrefs.GetInt("������ ID");
        //Debug.Log($"4. {PlayerPrefs.GetString("������ �̸�")}�� �ֺ� ���Կ��� ����!");

        if(database != null)
        {
            for(int i =0; i < database.itemList.Count; i++)
            {
                database.GetItemByID(i);
            }
            
            return database.GetItemByID(Itemindex);
        }
        else
        {
            Debug.Log($"Database�� instance�� null");
            return null;
        }

    }
    private void UpdateSlot(GameObject prefab, ItemData_hur itemdata)
    //�������� �������� �� �� ������ ������ �̸��� ������ �̹����� �°� ����� �Լ�
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

