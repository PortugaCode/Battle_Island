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

    [Header("UI")]
    [SerializeField] private GameObject rifleCrosshair; // 1인칭 라이플 시점 UI
    [SerializeField] private GameObject sniperCrosshair; // 1인칭 스나이퍼 시점 UI
    [SerializeField] private GameObject thirdPersonCrosshair; // 3인칭 크로스헤어 UI
    [SerializeField] private GameObject itemTextPrefab; // 텍스트 생성용 프리팹
    [SerializeField] private GameObject getItemUI; // 아이템 줍기 UI
    [SerializeField] private GameObject ammoText; // 총알 표시 UI
    [SerializeField] private GameObject damageUI; // 데미지 인디케이터 UI

    private bool isIndicatorOn = false;

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

    public void TurnOffUI()
    {
        rifleCrosshair.SetActive(false);
        sniperCrosshair.SetActive(false);
        thirdPersonCrosshair.SetActive(false);
    }

    public void ShowGetItemUI()
    {
        if (!getItemUI.activeSelf)
        {
            getItemUI.SetActive(true);
        }
        
        getItemUI.transform.Find("Text").GetComponent<Text>().text = $"{InventoryControl.instance.focusedItems[InventoryControl.instance.focusedItems.Count - 1].GetComponent<ItemControl>().itemName} 줍기";
    }

    public void CloseGetItemUI()
    {
        if (getItemUI.activeSelf)
        {
            getItemUI.SetActive(false);
        }
    }

    public void UpdateAmmoText(int ammo)
    {
        ammoText.GetComponent<Text>().text = $"{ammo} / {InventoryControl.instance.ammo}";
    }

    public void DamageIndicator(float damage)
    {
        if (!isIndicatorOn)
        {
            isIndicatorOn = true;
            StartCoroutine(IndicatorOn_co(damage));
        }
    }

    private IEnumerator IndicatorOn_co(float damage)
    {
        Color color = damageUI.GetComponent<Image>().color;

        float amount = 0;

        if (damage >= 20)
        {
            amount = 1.0f;
        }
        else
        {
            amount = (1.0f / 20.0f) * damage;
        }

        while (color.a < amount)
        {
            color.a += Time.deltaTime * 2.0f;
            damageUI.GetComponent<Image>().color = color;
            yield return null;
        }

        color.a = amount;
        damageUI.GetComponent<Image>().color = color;

        yield return new WaitForSeconds(1.0f);

        StartCoroutine(IndicatorOff_co());

        yield break;
    }

    private IEnumerator IndicatorOff_co()
    {
        Color color = damageUI.GetComponent<Image>().color;

        while (color.a > 0)
        {
            color.a -= Time.deltaTime;
            damageUI.GetComponent<Image>().color = color;
            yield return null;
        }

        color.a = 0f;
        damageUI.GetComponent<Image>().color = color;
        isIndicatorOn = false;
        yield break;
    }
}
