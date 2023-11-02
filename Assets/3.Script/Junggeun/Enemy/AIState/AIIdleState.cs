using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : MonoBehaviour, AIState
{
    float DuTime = 2.5f;
    float CoolTime = 2.5f;


    public AiStateID GetID()
    {
        return AiStateID.Idle;
    }

    public void Enter(AIAgent agent)
    {
        while(agent.navMeshAgent.speed != 0)
        {
            agent.navMeshAgent.speed -= 0.1f;
        }
    }

    public void AIUpdate(AIAgent agent)
    {
        CoolTime -= DuTime * Time.deltaTime;
        if(agent.isReady)
        {
            if (CoolTime > 0.0f)
            {
                Debug.Log(CoolTime);
                return;
            }
            agent.twoBoneIK.weight = 1f;
        }

        agent.stateMachine.ChangeState(AiStateID.ChasePlayer);

        Vector3 Playerdirection = agent.playerTarget.position - agent.transform.position;
        if(Playerdirection.magnitude > agent.config.maxSightDistance)
        {
            return;
        }

        Vector3 agnetDirection = agent.transform.forward;
        Playerdirection.Normalize();
        float dotProduct = Vector3.Dot(Playerdirection, agnetDirection);

        if(dotProduct > 0.0f)
        {
            agent.stateMachine.ChangeState(AiStateID.ChasePlayer);
        }


    }

    public void Exit(AIAgent agent)
    {

    }

}
