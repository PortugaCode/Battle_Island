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
    [SerializeField] private SurrItemClick surItemClick;

    //�ֺ� ������ �������� ��ƺ�ν� �κ��丮�� ������ �����
    [SerializeField] private Transform DiabloSpace;

    public bool ItemDragOn = false;

    private void Start()
    {
        surItemClick = FindObjectOfType<SurrItemClick>();
    }
    private void Update()
    {
        KnowItem();
    }

    private void KnowItem()
    //�������� �������� �� ������ ����
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ItemData_hur randomItem = GetRandomItem();
            GameObject SurSlotPrefab = Instantiate(slotprefab, parentTransform);
            UpdateSlot(SurSlotPrefab, randomItem);
        }
    }

    private ItemData_hur GetRandomItem()
    //���߿� ������ �� - itemList�� �ش��ϴ� �����۰� �̾����� ��
    {
        int randomindex = Random.Range(0, database.itemList.Count);
        return database.itemList[randomindex];
    }
    private void UpdateSlot(GameObject prefab, ItemData_hur itemdata)
    //�������� �������� �� �� ������ �̸��� �̹����� �°� ����� �Լ�
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
    public void ClickItem()
    {
        if (surItemClick.Click && !ItemDragOn)
        {
            surItemClick.Click = false;
            ItemDragOn = true;
            DestroyInSurr();
        }
        else
        {
            Debug.Log("ItemDragOn�� ���� false�� �ƴմϴ�");
        }
    }
    public void DestroyInSurr()
    //���� ��� �κ��丮Transform�� �θ� �ش� �����ո� �����ȴ�. 
    {
        Debug.Log("DestroyInSurr �Լ�!");

        //�ٽ� ������ �����Ǿ�� ��
        KnowItem();
    }
}
