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

    [SerializeField] private GameObject rifleCrosshair; // 1��Ī ������ ���� UI
    [SerializeField] private GameObject sniperCrosshair; // 1��Ī �������� ���� UI
    [SerializeField] private GameObject thirdPersonCrosshair; // 3��Ī ũ�ν���� UI
    [SerializeField] private GameObject inventoryPanel; // �κ��丮 �г�
    [SerializeField] private GameObject itemTextPrefab; // �ؽ�Ʈ ������ ������
    [SerializeField] private GameObject contentUI; // �ؽ�Ʈ�� ��� �θ� UI ������Ʈ

    public void FirstPersonRifleCrosshair(bool on) // Rifle UI Toggle
    {
        rifleCrosshair.SetActive(on);
    }

    public void FirstPersonSniperCrosshair(bool on) // Sniper UI Toggle
    {
        sniperCrosshair.SetActive(on);
    }

    public void ThirdPersonCrosshair(bool on) // 3��Ī Crosshair UI Toggle
    {
        thirdPersonCrosshair.SetActive(on);
    }

    public void ToggleInventory(List<GameObject> nearItems) // �κ��丮 �г� Toggle
    {
        if (inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(false);
        }
        else
        {
            UpdateNearItems(nearItems); // �κ��丮 �� �� �ֺ� ������ ��Ž��
            inventoryPanel.SetActive(true);
        }
    }

    public void TurnOffUI()
    {
        rifleCrosshair.SetActive(false);
        sniperCrosshair.SetActive(false);
        thirdPersonCrosshair.SetActive(false);
        inventoryPanel.SetActive(false);
    }

    private void UpdateNearItems(List<GameObject> nearItems)
    {
        for (int i = contentUI.transform.childCount - 1; i >= 0; i--) // ���� ����Ʈ Ŭ����
        {
            Transform child = contentUI.transform.GetChild(i);
            Destroy(child.gameObject);
        }

        for (int i = 0; i < nearItems.Count; i++) // �ֺ� ������ ����Ʈ�� �߰� �� �ؽ�Ʈ ���
        {
            GameObject currentText = Instantiate(itemTextPrefab, transform.position, Quaternion.identity, contentUI.transform);
            currentText.GetComponent<Text>().text = nearItems[i].name;
        }
    }
}
