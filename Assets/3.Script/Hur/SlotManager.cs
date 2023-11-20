using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotHolder; //���� �θ�
    [SerializeField] private PlayerController player;
    [SerializeField] private Database_hur database;
    [SerializeField] private ItemData_hur ItemData;

    public int count; //�ʱ� ���� ����

    public List<int> ItemID = new List<int>();

    private void Awake()
    {
        ItemID.Add(108);
        ItemID.Add(104);
        ItemID.Add(105);
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        database = FindObjectOfType<Database_hur>();
        
    }

    public void AddSlot()
    {
        for (int i = 0; i < ItemID.Count; i++)
        {
            MeetItem(ItemID[i]);
        }
    }

    private void MeetItem(int id) //������ �����ϸ� ���԰���++
    {
        count++;
        PrefabPlus(count, id);
    }

    private void PrefabPlus(int count, int id)
    {
        //Debug.Log("PrefabPlus");
        GameObject newSlot = Instantiate(slotPrefab, transform.position, Quaternion.identity);
        newSlot.transform.SetParent(slotHolder);
        newSlot.transform.position = slotHolder.position + new Vector3(count * 0f, 0f, 0f);

        // id �޾ƿ��ݾ� -> �߾�
        // �� id�� �´� �ε��� �˾Ƴ���
        // �� �ε��� (�ʰ� ��������� ������ ����Ʈ���� ���� id�� ��ġ�ϴ� �ε���) �˾�
        // newSlot�� ��� �̸�, ������<= �̰� ����� �ʰ� ������� ������ ����Ʈ�� ���ܾ�

        int index = database.ReturnItemIndex(id);
        newSlot.transform.GetChild(0).GetComponent<Image>().sprite = database.itemList[index].itemIcon;
        newSlot.transform.GetChild(2).GetComponent<Text>().text = database.itemList[index].itemName;
        newSlot.GetComponent<Slot>().id = database.itemList[index].itemID;
        newSlot.GetComponent<Slot>().width = database.itemList[index].width;
        newSlot.GetComponent<Slot>().height = database.itemList[index].height;
    }

    
}