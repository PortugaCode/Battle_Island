using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIState
{

    public AiStateID GetID()
    {
        return AiStateID.Idle;
    }

    public void Enter(AIAgent agent)
    {

    }

    public void Update(AIAgent agent)
    {
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
