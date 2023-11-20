using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIControll : MonoBehaviour
{
    [Header("Çï¸ä, ¹æ¾î±¸, °¡¹æ")]
    [SerializeField] private Image helmatImg;
    [SerializeField] private Image armorImg;
    [SerializeField] private Image bagImg;
    [SerializeField] private Image rifleImg1;

    public bool isHelmet = false;
    public bool isArmor = false;
    public bool isBag = false;
    public bool isRifle1 = false;

    private CombatControl combat;

    [Header("1¹øÀº ¹«±â, 2¹øÀº ¼ö·ùÅº")]
    [SerializeField] private Image rifleImg2;
    [SerializeField] private Image throwImg;

    public bool isRifle2 = false;
    public bool isThrow = false;


    [Header("ÃÑ¾Ë ³²Àº °Í")]
    public Text ammoText;

    [Header("¼ö·ùÅº °¹¼ö")]
    public Text throwText;

    [Header("HP ¹Ù")]
    public Slider hpbar;


    [Header("³²Àº Àû")]
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

        //===============================================

        rifleImg2.enabled = isRifle2;
        throwImg.enabled = isThrow;

        //===============================================

        hpbar.value = combat.playerHealth;
    }

    
}
