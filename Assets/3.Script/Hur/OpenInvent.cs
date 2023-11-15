using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInvent : MonoBehaviour
{
    [SerializeField] GameObject inventoryObj;
    [SerializeField] private Slot slot;
    public bool Open = false;

    private void Start()
    {
        inventoryObj.SetActive(false);
        slot = FindObjectOfType<Slot>();
    }
    private void Update()
    {
        if (!Open && Input.GetKeyDown(KeyCode.E))
        {
            Inventory_ON();
        }
        else if (Open && Input.GetKeyDown(KeyCode.Escape))
        {
            Inventory_OFF();
        }
        else if (Open && Input.GetKeyDown(KeyCode.E))
        {
            Inventory_OFF();
        }
    }
    public void Inventory_ON()
    {
        Open = true;
        inventoryObj.SetActive(true);
        slot.AddSlot();
    }
    public void Inventory_OFF()
    {
        Open = false;
        inventoryObj.SetActive(false);
    }
}
