using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIControll : MonoBehaviour
{
    [Header("���, ��, ����")]
    [SerializeField] private Image helmatImg;
    [SerializeField] private Image armorImg;
    [SerializeField] private Image bagImg;
    [SerializeField] private Image rifleImg1;

    public bool isHelmet = false;
    public bool isArmor = false;
    public bool isBag = false;
    public bool isRifle1 = false;

    private CombatControl combat;

    [Header("1���� ����, 2���� ����ź")]
    [SerializeField] private Image rifleImg2;
    [SerializeField] private Image throwImg;

    public bool isRifle2 = false;
    public bool isThrow = false;


    [Header("�Ѿ� ���� ��")]
    public Text ammoText;

    [Header("����ź ����")]
    public Text throwText;

    [Header("HP ��")]
    public Slider hpbar;


    [Header("���� ��")]
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
