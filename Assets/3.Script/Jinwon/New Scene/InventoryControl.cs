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
    public List<GameObject> focusedItems; // 현재 콜라이더에 접촉된 아이템 리스트

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
            // 필요한 정보 있으면 이후에 추가
        }
    }

    // [소지한 아이템 리스트]
    public List<Item> inventory;

    // [총알 개수]
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

        if (id == 103) // 아머
        {
            armorModel.SetActive(true);
        }
        else if (id == 104) // 가방
        {
            bagModel.SetActive(true);
        }
        else if (id == 108) // 총알상자
        {
            ammo += amount;
            UIManager.instance.UpdateAmmoText(0); // Test
        }
        else if (id == 111) // 헬멧
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
            Debug.Log($"아이템 이름 : {inventory[i].itemName}, 아이템 ID : {inventory[i].id}");
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
