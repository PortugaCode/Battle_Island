using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotHolder; //���� �θ�
    [SerializeField] private Database_hur database;
    [SerializeField] private ItemData_hur ItemData;

    public int count; //�ʱ� ���� ����

    public List<int> ItemID = new List<int>();

    public int id;

    private void Awake()
    {
        ItemID.Add(100);
        ItemID.Add(101);
        ItemID.Add(102);
        ItemID.Add(103);
        ItemID.Add(104);
        ItemID.Add(105);
        ItemID.Add(106);
        ItemID.Add(107);
        ItemID.Add(108);
        ItemID.Add(109);
        ItemID.Add(110);
        ItemID.Add(111);
        ItemID.Add(112);
        ItemID.Add(113);
        ItemID.Add(114);
        ItemID.Add(115);
        ItemID.Add(116);
        ItemID.Add(117);
    }

    private void Start()
    {
        //player = FindObjectOfType<PlayerController>();
        database = FindObjectOfType<Database_hur>();

    }

    public void AddSlot()
    {
        for (int i = 0; i < ItemID.Count; i++)
        {
            MeetItem(ItemID[i]);
        }
    }

    public void MeetItem(int id) //������ �����ϸ� ���԰���++
    {
        count++;
        PrefabPlus(count, id);

        //return id;
    }

    private void PrefabPlus(int count, int id)
    {
        //�ֺ� ����
        GameObject newSlot = Instantiate(slotPrefab, transform.position, Quaternion.identity);
        newSlot.transform.SetParent(slotHolder);
        newSlot.transform.position = slotHolder.position + new Vector3(count * 0f, 0f, 0f);

        // id �޾ƿ��ݾ� -> �߾�
        // �� id�� �´� �ε��� �˾Ƴ���
        // �� �ε��� (�ʰ� ��������� ������ ����Ʈ���� ���� id�� ��ġ�ϴ� �ε���) �˾�
        // newSlot�� ��� �̸�, ������<= �̰� ����� �ʰ� ������� ������ ����Ʈ�� ���ܾ�

        //�ֺ� ����
        int index = database.ReturnItemIndex(id);
        newSlot.transform.GetChild(0).GetComponent<Image>().sprite = database.itemList[index].itemIcon;
        newSlot.transform.GetChild(2).GetComponent<Text>().text = database.itemList[index].itemName;
        newSlot.GetComponent<Slot>().id = database.itemList[index].itemID;
        newSlot.GetComponent<Slot>().width = database.itemList[index].width;
        newSlot.GetComponent<Slot>().height = database.itemList[index].height;

        //������ Ÿ��
        //newSlot.GetComponent<Slot>().usingType = database.itemList[index].usingType;
        
        //CheckType(id);
    }
}
