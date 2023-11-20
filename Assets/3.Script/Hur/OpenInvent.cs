using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInvent : MonoBehaviour
{
    [SerializeField] GameObject inventoryObj;
    [SerializeField] private SlotManager slot;
    [SerializeField] private GameObject slotHolder;
    public bool Open = false;
    public bool openThisSC = false; //다른 스크립트에게 신호

    private void Start()
    {
        inventoryObj.SetActive(false);
        slot = FindObjectOfType<SlotManager>();
    }
    private void Update()
    {
        if (!Open && !openThisSC&& Input.GetKeyDown(KeyCode.E))
        {
            Inventory_ON();
        }
        else if (Open && openThisSC && Input.GetKeyDown(KeyCode.Escape))
        {
            Inventory_OFF();
        }
        else if (Open && openThisSC && Input.GetKeyDown(KeyCode.E))
        {
            Inventory_OFF();
        }
    }
    public void Inventory_ON()
    {
        Open = true;
        openThisSC = true;

        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            Destroy(slotHolder.transform.GetChild(i).gameObject);
        }

        inventoryObj.SetActive(true);
        slot.AddSlot();
    }
    public void Inventory_OFF()
    {
        Open = false;
        openThisSC = false;
        inventoryObj.SetActive(false);
    }
}
