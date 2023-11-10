using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HelicAgent : MonoBehaviour
{
    [HideInInspector] public HelicStateMachine stateMachine;

    [Header("Helicopter StartState")]
    public HelicStateID initalState;


    [Header("TargetAim")]
    public Transform OriginalTarget;
    public Transform AimTarget;
    public Transform BodyTarget;
    public Transform PositionTarget;

    private void Awake()
    {
        stateMachine = new HelicStateMachine(this);

        #region 상태추가

        stateMachine.RegsisterState(new HelicRandomMoveState());
        stateMachine.RegsisterState(new HelicChasePlayerState());

        #endregion


        stateMachine.ChangeState(initalState);
    }

    private void Update()
    {
        stateMachine.Update();
    }
}
