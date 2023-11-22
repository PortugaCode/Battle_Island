using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateBulletCountUI : MonoBehaviour
{
    private void Update()
    {
        GetComponent<Text>().text = $"Ammo : {InventoryControl.instance.ammo}";
    }
}
