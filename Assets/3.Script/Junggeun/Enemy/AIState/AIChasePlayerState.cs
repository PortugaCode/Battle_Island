using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChasePlayerState : AIState
{

    
    private float timer = 0.0f;

    public AiStateID GetID()
    {
        return AiStateID.ChasePlayer;
    }

    public void Enter(AIAgent agent)
    {
        agent.AimTarget.position = agent.playerTarget.position;
        agent.navMeshAgent.speed = 3f;
    }

    public void AIUpdate(AIAgent agent)
    {
        if (agent.enemyHealth.IsDie)
        {
            agent.navMeshAgent.velocity = Vector3.zero;
            return;
        }


        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            float sqdistance = (agent.playerTarget.position - agent.navMeshAgent.destination).sqrMagnitude;
            if (sqdistance > agent.config.maxDistance * agent.config.maxDistance) // ������ �ϴ� ������ SqrMagitude�� ������ �� ���� ��ȯ�ϱ� ������
            {
                agent.navMeshAgent.destination = agent.playerTarget.position;
            }
            timer = agent.config.maxTime;
        }
    }

    public void Exit(AIAgent agent)
    {
        
    }


}
