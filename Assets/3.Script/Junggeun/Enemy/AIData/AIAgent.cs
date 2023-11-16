using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;


public class AIAgent : MonoBehaviour
{
    public bool isReady = false;
    public bool isAmmoReady = false;
    public bool isShot = false;
    public bool isRun = true;
    public bool isArmor;

    [HideInInspector] public AIStateMachine stateMachine;
    [HideInInspector] public EnemyHealth enemyHealth;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public RagDoll ragDoll;
    [HideInInspector] public SkinnedMeshRenderer mesh;
    [HideInInspector] public UIHealthBar ui;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Transform playerTarget_Tr;
    public Transform playerTarget;

    [HideInInspector] public bool isneedReload = false;
    [HideInInspector] public bool isnowReload = false;
    [HideInInspector] public RaycastHit hit;
    [HideInInspector] public EnemyAudio enemyAudio;

    [Header("Character")]
    [SerializeField] private GameObject[] EnemyModel;


    [Header("Gun Data")]
    public GunData[] gundata;
    public GunData Nowgundata;
    public int ammoRemain; //���� ��ü ź��
    public int magAmmo; // ���� źâ�� ���� �ִ� ź��



    [Header("FireEffect")]
    public ParticleSystem FireEffect;
    public ParticleSystem FireEffect1;
    public GameObject FireLight;


    [Header("TargetAim")]
    public Transform AimTarget;
    public Transform originTarget;

    [Header("Rigging")]
    public Rig rig;
    public TwoBoneIKConstraint twoBoneIK;

    [Header("rifleData")]
    //public GameObject[] rifleWeapons;
    //public Transform[] StartAim;
    public GameObject CurrentGun;
    public Gun CurrentGun_Gun;
    public GameObject GunPivot;
    public GameObject Bullet;
    //[HideInInspector] public GameObject SelectRifleWeapons;
    //[HideInInspector] public Transform SelectStartAim;

    [Header("ArmorData")]
    public GameObject[] Armor;
    [HideInInspector] public GameObject SelectArmor;

    [Header("WallLayer")]
    public LayerMask WallLayer;

    [Header("PlayerLayer")]
    public LayerMask PlayerLayer;

    [Header("AIDefaultState")]
    public AiStateID initalState;
    public AIAgentConfig config;

    private void Awake()
    {

        //�� SetActive false �۾�
        for (int i = 0; i < Armor.Length; i++)
        {
            Armor[i].SetActive(false);
        }

        int a = Random.Range(0, 8);
        EnemyModel[a].SetActive(true);

        //===========================================================================
        TryGetComponent(out enemyHealth);
        TryGetComponent(out navMeshAgent);
        TryGetComponent(out ragDoll);
        TryGetComponent(out enemyAudio);
        TryGetComponent(out animator);
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        ui = GetComponentInChildren<UIHealthBar>();
        if(GameObject.FindGameObjectWithTag("Player"))
        {
            GameObject.FindGameObjectWithTag("Player").TryGetComponent(out playerTarget); //���߿� �ٲ����
        }
        //GameObject.FindGameObjectWithTag("PlayerTarget").TryGetComponent(out playerTarget);
        //GameObject.FindGameObjectWithTag("Player").TryGetComponent(out playerTarget);
        //===========================================================================


        //AIStateMachine �ʱ�ȭ
        stateMachine = new AIStateMachine(this);

        #region �����߰�
        stateMachine.RegsisterState(new AIChasePlayerState());
        stateMachine.RegsisterState(new AIDeathState());
        stateMachine.RegsisterState(new AIIdleState());
        stateMachine.RegsisterState(new AIFindWeaponState());
        stateMachine.RegsisterState(new AIShootingState());
        stateMachine.RegsisterState(new AIRandomMoveState());
        stateMachine.RegsisterState(new AIReloadState());
        stateMachine.RegsisterState(new AIFindBulletState());
        stateMachine.RegsisterState(new AIFindArmorState());
        stateMachine.RegsisterState(new AIStandbyState());
        stateMachine.RegsisterState(new AIRuntoWall());
        stateMachine.RegsisterState(new AIAlreadyDieState());
        #endregion


        // �ʱ� ���� ����
        stateMachine.ChangeState(initalState);
    }

    private void Update()
    {

        stateMachine.Update();
        //playerTarget.position = playerTarget_Tr.position + new Vector3(0f, 1.2f, 0f);

        if (isShot && magAmmo > 0 && isReady && isAmmoReady)
        {
            StartCoroutine(ShotEffect());
        }
        isShot = false;

        if (isneedReload && !isnowReload && isReady && isAmmoReady)
        {
            StartCoroutine(ReloadCo());
        }
    }

    private IEnumerator ShotEffect()
    {
        /*        lineRenderer.SetPosition(0, SelectStartAim.position);
                lineRenderer.SetPosition(1, hitPosition);
                lineRenderer.enabled = true;
                lineRenderer.enabled = false;
        */
        FireLight.transform.position = CurrentGun_Gun.muzzleTransform.position;
        FireLight.transform.rotation = CurrentGun_Gun.muzzleTransform.rotation;
        FireLight.SetActive(true);
        yield return new WaitForSeconds(0.03f);
        FireLight.SetActive(false);
        isShot = false;
    }

    private IEnumerator ReloadCo()
    {
        isnowReload = true;
        enemyAudio.PlayReload();
        yield return new WaitForSeconds(Nowgundata.reloadTime);

        int ammoToFill = Nowgundata.magCapcity - magAmmo;

        if(ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }

        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;

        isnowReload = false;
        isneedReload = false;
    }

}
