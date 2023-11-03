using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;


public class AIAgent : MonoBehaviour
{
    public bool isReady = false;

    [HideInInspector] public AIStateMachine stateMachine;
    [HideInInspector] public EnemyHealth enemyHealth;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public RagDoll ragDoll;
    [HideInInspector] public SkinnedMeshRenderer mesh;
    [HideInInspector] public UIHealthBar ui;
    [HideInInspector] public Transform playerTarget;
    [HideInInspector] public GameObject SelectRifleWeapons;
    [HideInInspector] public Transform SelectStartAim;
    [HideInInspector] public bool isShot;
    [HideInInspector] public RaycastHit hit;

    [Header("FireEffect")]
    public ParticleSystem FireEffect;
    public ParticleSystem FireEffect1;
    public GameObject FireLight;
    public LineRenderer lineRenderer;


    [Header("TargetAim")]
    public Transform AimTarget;
    public Transform originTarget;

    [Header("Rigging")]
    public Rig rig;
    public TwoBoneIKConstraint twoBoneIK;

    [Header("rifleData")]
    public GameObject[] rifleWeapons;
    public Transform[] StartAim;
    public GameObject Bullet;

    [Header("WallLayer")]
    public LayerMask WallLayer;

    [Header("AIDefaultState")]
    public AiStateID initalState;
    public AIAgentConfig config;

    private void Awake()
    {
        //총기 SetActive false 작업
        for (int i =0; i < rifleWeapons.Length; i++)
        {
            rifleWeapons[i].SetActive(false);
        }

        //===========================================================================
        TryGetComponent(out enemyHealth);
        TryGetComponent(out navMeshAgent);
        TryGetComponent(out ragDoll);
        TryGetComponent(out lineRenderer);
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        ui = GetComponentInChildren<UIHealthBar>();
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out playerTarget);
        //===========================================================================


        //AIStateMachine 초기화
        stateMachine = new AIStateMachine(this);

        #region 상태추가
        stateMachine.RegsisterState(new AIChasePlayerState());
        stateMachine.RegsisterState(new AIDeathState());
        stateMachine.RegsisterState(new AIIdleState());
        stateMachine.RegsisterState(new AIFindWeaponState());
        stateMachine.RegsisterState(new AIShootingState());
        stateMachine.RegsisterState(new AIRandomMoveState());
        #endregion


        // 초기 상태 실행
        stateMachine.ChangeState(initalState);
    }

    private void Update()
    {
        stateMachine.Update();
        if(isShot)
        {
            StartCoroutine(ShotEffect(hit.point));
        }
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        Debug.Log("작동중");
        lineRenderer.SetPosition(0, StartAim[2].transform.position);
        lineRenderer.SetPosition(1, hitPosition);
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.03f);
        lineRenderer.enabled = false;
    }

}
