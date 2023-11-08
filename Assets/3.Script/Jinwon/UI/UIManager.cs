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

    [SerializeField] private GameObject rifleCrosshair; // 1인칭 라이플 시점 UI
    [SerializeField] private GameObject sniperCrosshair; // 1인칭 스나이퍼 시점 UI
    [SerializeField] private GameObject thirdPersonCrosshair; // 3인칭 크로스헤어 UI
    [SerializeField] private GameObject inventoryPanel; // 인벤토리 패널
    [SerializeField] private GameObject itemTextPrefab; // 텍스트 생성용 프리팹
    [SerializeField] private GameObject contentUI; // 텍스트가 담길 부모 UI 오브젝트

    public void FirstPersonRifleCrosshair(bool on) // Rifle UI Toggle
    {
        rifleCrosshair.SetActive(on);
    }

    public void FirstPersonSniperCrosshair(bool on) // Sniper UI Toggle
    {
        sniperCrosshair.SetActive(on);
    }

    public void ThirdPersonCrosshair(bool on) // 3인칭 Crosshair UI Toggle
    {
        thirdPersonCrosshair.SetActive(on);
    }

    public void ToggleInventory(List<GameObject> nearItems) // 인벤토리 패널 Toggle
    {
        if (inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(false);
        }
        else
        {
            UpdateNearItems(nearItems); // 인벤토리 열 때 주변 아이템 재탐색
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
        for (int i = contentUI.transform.childCount - 1; i >= 0; i--) // 이전 리스트 클리어
        {
            Transform child = contentUI.transform.GetChild(i);
            Destroy(child.gameObject);
        }

        for (int i = 0; i < nearItems.Count; i++) // 주변 아이템 리스트에 추가 후 텍스트 출력
        {
            GameObject currentText = Instantiate(itemTextPrefab, transform.position, Quaternion.identity, contentUI.transform);
            currentText.GetComponent<Text>().text = nearItems[i].name;
        }
    }
}
