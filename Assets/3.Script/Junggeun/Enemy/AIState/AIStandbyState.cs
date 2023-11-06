using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStandbyState : AIState
{
    private Animator animator;

    public AiStateID GetID()
    {
        return AiStateID.Standby;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("��� ���");
        animator = agent.GetComponent<Animator>();
        while (agent.navMeshAgent.speed != 0)
        {
            agent.navMeshAgent.speed -= 0.1f;
        }
        animator.SetTrigger("Crouch");
    }

    public void AIUpdate(AIAgent agent)
    {

        agent.AimTarget.position = Vector3.Lerp(agent.AimTarget.position, agent.originTarget.position, 4f * Time.deltaTime);
        Vector3 Playerdirection = agent.playerTarget.position - agent.transform.position;
        if (Playerdirection.magnitude > agent.config.maxSightDistance)
        {
            return;
        }

        Vector3 agnetDirection = agent.transform.forward;
        Playerdirection.Normalize();
        //���� ���
        /*
        ���� > 0 �̸�, �� < 90 (ĳ���� ����)
        ���� < 0 �̸�, �� > 90 (ĳ���� �Ĺ�)
        ���� = 0 �̸�, �� = 90
         */
        float dotProduct = Vector3.Dot(Playerdirection, agnetDirection);

        if (dotProduct > 0.0f)
        {
            agent.stateMachine.ChangeState(AiStateID.Shooting);
        }
    }

    public void Exit(AIAgent agent)
    {

    }
}
