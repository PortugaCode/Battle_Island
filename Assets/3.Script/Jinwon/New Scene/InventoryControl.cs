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
    private List<GameObject> nearItemList = new List<GameObject>(); // 주변 아이템 리스트
    public LayerMask itemLayer; // 아이템 체크할 레이어
    public GameObject focusedItem; // 현재 포커스된 아이템

    public struct Item
    {
        public string name;
        public int id;

        public Item(string name, int id)
        {
            this.name = name;
            this.id = id;
            // 필요한 정보 있으면 이후에 추가
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
                // 존재한다
                return true;
            }
        }

        // 존재하지 않는다
        return false;
    }

    private void GetNearItemList()
    {
        nearItemList.Clear();

        Collider[] colliders = Physics.OverlapSphere(transform.position, 3.0f, itemLayer); // 플레이어 주변의 item레이어 오브젝트들 검출

        foreach (Collider c in colliders)
        {
            nearItemList.Add(c.gameObject); // 주변 아이템들 리스트에 추가
        }
    }
}
