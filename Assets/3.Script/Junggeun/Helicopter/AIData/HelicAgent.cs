using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HelicAgent : MonoBehaviour
{
    [HideInInspector] public HelicStateMachine stateMachine;
    [HideInInspector] public NavMeshAgent navMeshAgent;

    [Header("Helicopter StartState")]
    public HelicStateID initalState;


    [Header("TargetAim")]
    public Transform AimTarget;
    public Transform BodyTarget;

    private void Awake()
    {
        TryGetComponent(out navMeshAgent);

        stateMachine = new HelicStateMachine(this);

        stateMachine.ChangeState(initalState);
    }

    private void Update()
    {
        stateMachine.Update();
    }
}
