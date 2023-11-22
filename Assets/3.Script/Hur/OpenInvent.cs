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

    // 스크립트 참조
    private ZoomControl zoomControl;
    private CharacterMovement characterMovement;

    private void Start()
    {
        StartCoroutine(DisableInventory_co());
        slot = FindObjectOfType<SlotManager>();
        zoomControl = FindObjectOfType<ZoomControl>();
        characterMovement = FindObjectOfType<CharacterMovement>();
    }
    private void Update()
    {
        if (!Open && !openThisSC&& Input.GetKeyDown(KeyCode.Tab))
        {
            Inventory_ON();
        }
        else if (Open && openThisSC && Input.GetKeyDown(KeyCode.Escape))
        {
            Inventory_OFF();
        }
        else if (Open && openThisSC && Input.GetKeyDown(KeyCode.Tab))
        {
            Inventory_OFF();
        }
    }
    public void Inventory_ON()
    {
        InventoryControl.instance.GetNearItemList();

        Open = true;
        openThisSC = true;

        zoomControl.StopCameraMove();
        characterMovement.canMove = false;
        characterMovement.x = 0f;
        characterMovement.z = 0f;
        characterMovement.animator.SetFloat("MoveSpeedZ", 0f);
        characterMovement.animator.SetFloat("MoveSpeedX", 0f);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

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

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        inventoryObj.SetActive(false);

        zoomControl.StartCameraMove();
        characterMovement.canMove = true;
    }

    private IEnumerator DisableInventory_co()
    {
        yield return new WaitForSeconds(0.05f);

        inventoryObj.SetActive(false);
    }
}
