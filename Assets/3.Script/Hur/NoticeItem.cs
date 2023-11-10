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
    [SerializeField] private ItemTrigger itemTrigger;

    public bool ItemDragOn = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        database = FindObjectOfType<Database_hur>();
        itemTrigger = FindObjectOfType<ItemTrigger>();
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
        Debug.Log("������ ���� in NoticeItem");

        //ItemData_hur randomItem = GetRandomItem();
        //GameObject SurSlotPrefab = Instantiate(slotprefab, parentTransform);
        //UpdateSlot(SurSlotPrefab, randomItem);

        ItemData_hur itemData = MatchInfo();
        GameObject SurSlotPrefab = Instantiate(slotprefab, parentTransform);
        UpdateSlot(SurSlotPrefab, itemData);
    }

    private ItemData_hur MatchInfo()
    //itemList�� �ش��ϴ� �����۰� �̾����� ��
    {
        //int Itemindex = Random.Range(0, database.itemList.Count);
        ////Debug.Log("������ ����!");
        //return database.itemList[Itemindex];

        int Itemindex = itemTrigger.PlayerPrefs.SetInt;
        Debug.Log("������ ����!");
        return database.itemList[Itemindex];
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

