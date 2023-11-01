using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;


public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public EnemyHealth enemyHealth;
    public NavMeshAgent navMeshAgent;
    public RagDoll ragDoll;
    public SkinnedMeshRenderer mesh;
    public UIHealthBar ui;
    public Transform playerTarget;
    public Transform AimTarget;
    public Rig rig;
    public TwoBoneIKConstraint twoBoneIK;

    public GameObject[] rifleWeapons;

    public AiStateID initalState;
    public AIAgentConfig config;

    private void Awake()
    {
        for(int i =0; i < rifleWeapons.Length; i++)
        {
            rifleWeapons[i].SetActive(false);
        }
        TryGetComponent(out enemyHealth);
        TryGetComponent(out navMeshAgent);
        TryGetComponent(out ragDoll);
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        ui = GetComponentInChildren<UIHealthBar>();
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out playerTarget);

        stateMachine = new AIStateMachine(this);
        stateMachine.RegsisterState(new AIChasePlayerState());
        stateMachine.RegsisterState(new AIDeathState());
        stateMachine.RegsisterState(new AIIdleState());
        stateMachine.RegsisterState(new AIFindWeaponState());
        stateMachine.ChangeState(initalState);
    }

    private void Update()
    {
        stateMachine.Update();
    }
}
