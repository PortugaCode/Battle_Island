using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStandbyState : AIState
{
    private Animator animator;
    private float lastFireTime;

    public AiStateID GetID()
    {
        return AiStateID.Standby;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("��� ���");
        animator = agent.GetComponent<Animator>();
        if (agent.ammoRemain <= 0)
        {
            return;
        }
        animator.SetTrigger("Crouch");
        animator.SetTrigger("Reload");
        agent.isAmmoReady = true;
        agent.isneedReload = true;
    }

    public void AIUpdate(AIAgent agent)
    {
        agent.AimTarget.position = Vector3.Lerp(agent.AimTarget.position, agent.playerTarget.position, 3f * Time.deltaTime);
        agent.transform.LookAt(agent.playerTarget);
        Vector3 Playerdirection = agent.playerTarget.position - agent.SelectStartAim.position;
        if (Playerdirection.magnitude < agent.config.maxSightDistance + 5f)
        {
            agent.stateMachine.ChangeState(AiStateID.Shooting);
        }

/*        Vector3 agnetDirection = agent.transform.forward;
        Playerdirection.Normalize();
        //���� ���
        *//*
        ���� > 0 �̸�, �� < 90 (ĳ���� ����)
        ���� < 0 �̸�, �� > 90 (ĳ���� �Ĺ�)
        ���� = 0 �̸�, �� = 90
         *//*
        float dotProduct = Vector3.Dot(Playerdirection, agnetDirection);

        if (dotProduct > 0.0f)
        {
            //agent.stateMachine.ChangeState(AiStateID.Shooting);
        }*/
    }

    public void Exit(AIAgent agent)
    {

    }
}
