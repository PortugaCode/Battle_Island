using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public EnemyHealth enemyHealth;
    public NavMeshAgent navMeshAgent;
    public RagDoll ragDoll;
    public SkinnedMeshRenderer mesh;
    public UIHealthBar ui;


    public AiStateID initalState;
    public AIAgentConfig config;

    private void Awake()
    {
        TryGetComponent(out enemyHealth);
        TryGetComponent(out navMeshAgent);
        TryGetComponent(out ragDoll);
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        ui = GetComponentInChildren<UIHealthBar>();

        stateMachine = new AIStateMachine(this);
        stateMachine.RegsisterState(new AIChasePlayerState());
        stateMachine.RegsisterState(new AIDeathState());
        stateMachine.ChangeState(initalState);
    }

    private void Update()
    {
        stateMachine.Update();
    }
}
