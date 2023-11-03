using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

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
    }

    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject itemTextPrefab;
    [SerializeField] private GameObject contentUI;

    public void Crosshair(bool on)
    {
        crosshair.SetActive(on);
    }

    public void ToggleInventory(List<GameObject> nearItems) // 牢亥配府 菩澄 on off
    {
        if (inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(false);
        }
        else
        {
            UpdateNearItems(nearItems);
            inventoryPanel.SetActive(true);
        }
    }

    private void UpdateNearItems(List<GameObject> nearItems)
    {
        for (int i = contentUI.transform.childCount - 1; i >= 0; i--) // 固府 积己等 府胶飘 力芭
        {
            Transform child = contentUI.transform.GetChild(i);
            Destroy(child.gameObject);
        }

        for (int i = 0; i < nearItems.Count; i++) // 府胶飘 积己
        {
            GameObject currentText = Instantiate(itemTextPrefab, transform.position, Quaternion.identity, contentUI.transform);
            currentText.GetComponent<Text>().text = nearItems[i].name;
        }
    }
}
