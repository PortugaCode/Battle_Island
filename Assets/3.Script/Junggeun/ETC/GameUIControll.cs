using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIControll : MonoBehaviour
{
    [Header("헬멧, 방어구, 가방")]
    [SerializeField] private Image helmatImg;
    [SerializeField] private Image armorImg;
    [SerializeField] private Image bagImg;
    [SerializeField] private Image rifleImg1;

    public bool isHelmet = false;
    public bool isArmor = false;
    public bool isBag = false;
    public bool isRifle1 = false;

    private CombatControl combat;

    [Header("1번은 무기, 2번은 수류탄")]
    [SerializeField] private Image rifleImg2;
    [SerializeField] private Image throwImg;


    [Header("총알 남은 것")]
    public Text ammoText;

    /*[Header("수류탄 갯수")]
    public Text throwText;*/

    [Header("HP 바")]
    public Slider hpbar;


    [Header("남은 적")]
    public Text countEnemy;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out combat);

    }

    private void Update()
    {
        helmatImg.enabled = isHelmet;
        armorImg.enabled = isArmor;
        bagImg.enabled = isBag;
        rifleImg1.enabled = isRifle1;

        if (combat.currentGun == null) ammoText.text = $"0 1 {InventoryControl.instance.ammo.ToString()}";

        else ammoText.text = $"{combat.currentGun.GetComponent<Gun>().currentMag} 1 {InventoryControl.instance.ammo.ToString()}";



        //===============================================

        rifleImg2.enabled = combat.isEquip_Gun;
        throwImg.enabled = combat.isEquip_Grenade;

        //===============================================

        //hpbar.maxValue = 360;
        hpbar.value = combat.playerHealth;

        //===============================================

        countEnemy.text = $"남은 적 : {GameManager.instance.enemyCount}";
    }

    
}
