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
    public List<GameObject> focusedItems; // ���� �ݶ��̴��� ���˵� ������ ����Ʈ

    [Header("Model")]
    [SerializeField] private GameObject bagModel;
    [SerializeField] private GameObject armorModel;
    [SerializeField] private GameObject helmetModel;

    public struct Item
    {
        public string itemName;
        public int id;
        public int amount;

        public Item(string name, int id, int amount)
        {
            this.itemName = name;
            this.id = id;
            this.amount = amount;
            // �ʿ��� ���� ������ ���Ŀ� �߰�
        }
    }

    // [������ ������ ����Ʈ]
    public List<Item> inventory;

    // [�Ѿ� ����]
    public int ammo;

    private void Update()
    {
        if (focusedItems.Count > 0 && Input.GetKeyDown(KeyCode.F))
        {
            focusedItems[focusedItems.Count - 1].GetComponent<ItemControl>().PickUpItem();
        }
    }

    public void GetItem(string name, int id, int amount)
    {
        Item currentItem = new Item(name, id, amount);

        if (id == 103) // �Ƹ�
        {
            armorModel.SetActive(true);
        }
        else if (id == 104) // ����
        {
            bagModel.SetActive(true);
        }
        else if (id == 108) // �Ѿ˻���
        {
            ammo += amount;
            UIManager.instance.UpdateAmmoText(0); // Test
        }
        else if (id == 111) // ���
        {
            helmetModel.SetActive(true);
        }

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
            Debug.Log($"������ �̸� : {inventory[i].itemName}, ������ ID : {inventory[i].id}");
        }
    }

    public bool CheckInventory(int id)
    {
        foreach (Item item in inventory)
        {
            if (item.id == id)
            {
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
