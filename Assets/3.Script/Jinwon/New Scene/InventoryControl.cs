using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryControl : MonoBehaviour
{
    public static InventoryControl instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        inventory = new List<Item>();
    }

    [Header("Item")]
    private List<GameObject> nearItemList = new List<GameObject>(); // �ֺ� ������ ����Ʈ
    public LayerMask itemLayer; // ������ üũ�� ���̾�
    public GameObject focusedItem; // ���� ��Ŀ���� ������

    public struct Item
    {
        public string name;
        public int id;

        public Item(string name, int id)
        {
            this.name = name;
            this.id = id;
            // �ʿ��� ���� ������ ���Ŀ� �߰�
        }
    }

    public List<Item> inventory;

    private void Update()
    {
        if (focusedItem != null && Input.GetKeyDown(KeyCode.F))
        {
            focusedItem.GetComponent<ItemControl>().PickUpItem();
        }
    }

    public void GetItem(string name, int id)
    {
        Item currentItem = new Item(name, id);
        inventory.Add(currentItem);
    }

    public void RemoveItem(int id)
    {
        foreach(Item item in inventory)
        {
            if (item.id == id)
            {
                inventory.Remove(item);
                return;
            }
        }
    }
    
    public void ShowInventory() // TEST
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            Debug.Log(inventory[i].name);
        }
    }

    public bool CheckInventory(int id)
    {
        foreach (Item item in inventory)
        {
            if (item.id == id)
            {
                // �����Ѵ�
                return true;
            }
        }

        // �������� �ʴ´�
        return false;
    }

    private void GetNearItemList()
    {
        nearItemList.Clear();

        Collider[] colliders = Physics.OverlapSphere(transform.position, 3.0f, itemLayer); // �÷��̾� �ֺ��� item���̾� ������Ʈ�� ����

        foreach (Collider c in colliders)
        {
            nearItemList.Add(c.gameObject); // �ֺ� �����۵� ����Ʈ�� �߰�
        }
    }
}
