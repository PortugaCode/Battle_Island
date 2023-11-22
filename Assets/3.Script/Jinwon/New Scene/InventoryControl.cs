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

        gameUIControll = FindObjectOfType<GameUIControll>();
        inventoryController_C = FindObjectOfType<InventoryController_C>();
        inventory = new List<Item>();
    }

    [Header("Item")]
    public List<GameObject> nearItemList = new List<GameObject>(); // �ֺ� ������ ����Ʈ
    public LayerMask itemLayer; // ������ üũ�� ���̾�
    public List<GameObject> focusedItems; // ���� �ݶ��̴��� ���˵� ������ ����Ʈ

    [Header("Model")]
    [SerializeField] private GameObject bagModel;
    [SerializeField] private GameObject armorModel;
    [SerializeField] private GameObject helmetModel;

    [Header("UI")]
    [SerializeField] private GameUIControll gameUIControll;
    private InventoryController_C inventoryController_C;

    public struct Item
    {
        public int id;
        public int amount;

        public Item(int id, int amount)
        {
            this.id = id;
            this.amount = amount;
            // �ʿ��� ���� ������ ���Ŀ� �߰�
        }
    }

    // [������ �����۵��� �����س��� ����Ʈ]
    public List<Item> inventory;

    // [�Ѿ� ����]
    public int ammo;

    // [���� �Ѿ˹ڽ��� ����]
    public int ammoBoxCount = 0;

    private void Update()
    {
        if (GetComponent<CharacterMovement>().canMove && focusedItems.Count > 0 && Input.GetKeyDown(KeyCode.F)) // TEST
        {
            if (focusedItems[focusedItems.Count - 1].CompareTag("Weapon"))
            {
                if (focusedItems[focusedItems.Count - 1].GetComponent<EquipCheck>().isEquip)
                {
                    return;
                }
            }



            inventoryController_C.AddItemOnUI(focusedItems[focusedItems.Count - 1].GetComponent<ItemControl>().x, focusedItems[focusedItems.Count - 1].GetComponent<ItemControl>().y, focusedItems[focusedItems.Count - 1].GetComponent<ItemControl>().id);
            focusedItems[focusedItems.Count - 1].GetComponent<ItemControl>().PickUpItem();
        }
    }

    public void GetItem(int id, int amount)
    {
        Item currentItem = new Item(id, amount);

        if (id == 104) // ����
        {
            if (!bagModel.activeSelf) // ���� �� Ȱ��ȭ
            {
                bagModel.SetActive(true);
            }
        }
        else if (id == 107) // ����ź
        {
            for (int i = 0; i < amount - 1; i++)
            {
                inventory.Add(currentItem);
            }
        }
        else if (id == 108) // �Ѿ˻���
        {
            ammoBoxCount += 1;
            ammo += 30;
            UIManager.instance.UpdateAmmoText(0); // Test
        }

        inventory.Add(currentItem);
    }

    public void EquipItem(int id)
    {
        if (id == 103) // �Ƹ�
        {
            if (!armorModel.activeSelf) // �Ƹ� �� Ȱ��ȭ
            {
                gameUIControll.isArmor = true;
                GetComponent<CombatControl>().isArmor = true;
                GetComponent<CombatControl>().playerHealth += 60;
                gameUIControll.hpbar.maxValue = 360.0f;
                armorModel.SetActive(true);
            }
        }
        else if (id == 111) // ���
        {
            if (!helmetModel.activeSelf) // ��� �� Ȱ��ȭ
            {
                gameUIControll.isHelmet = true;
                helmetModel.SetActive(true);
            }
        }
        else if (id == 116) // ������
        {
            gameUIControll.isRifle1 = true;
            GetComponent<CombatControl>().EquipGun(GunType.Rifle1);
        }
        else if (id == 117) // ��������
        {
            gameUIControll.isRifle1 = true;
            GetComponent<CombatControl>().EquipGun(GunType.Sniper1);
        }
    }

    public void UnEquipItem(int id)
    {
        if (id == 103) // �Ƹ�
        {
            gameUIControll.isArmor = false;
        }
        else if (id == 111) // ���
        {
            gameUIControll.isHelmet = false;
        }
        else if (id == 116) // ������
        {
            gameUIControll.isRifle1 = false;
            GetComponent<CombatControl>().UnEquipGun(GunType.Rifle1);
        }
        else if (id == 117) // ��������
        {
            gameUIControll.isRifle1 = false;
            GetComponent<CombatControl>().UnEquipGun(GunType.Sniper1);
        }
    }

    public void RemoveItem(int id)
    {
        foreach(Item item in inventory)
        {
            if (item.id == id)
            {
                inventory.Remove(item);
                inventoryController_C.RemoveItemOnUI(id);
                return;
            }
        }
    }
    
    public void RemoveItemFromNearList(int id)
    {
        foreach (GameObject item in nearItemList)
        {
            if (item != null && item.GetComponent<ItemControl>() != null)
            {
                if (item.GetComponent<ItemControl>().id == id)
                {
                    nearItemList.Remove(item);
                    focusedItems.Remove(item);
                    Destroy(item);
                    return;
                }
            }
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

    public void GetNearItemList()
    {
        nearItemList.Clear();

        Collider[] colliders = Physics.OverlapSphere(transform.position, 3.0f, itemLayer); // �÷��̾� �ֺ��� item���̾� ������Ʈ�� ����

        foreach (Collider c in colliders)
        {
            nearItemList.Add(c.gameObject); // �ֺ� �����۵� ����Ʈ�� �߰�
        }
    }
}
