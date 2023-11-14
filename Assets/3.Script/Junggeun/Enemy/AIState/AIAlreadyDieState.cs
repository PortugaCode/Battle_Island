using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAlreadyDieState : AIState
{
    public AiStateID GetID()
    {
        return AiStateID.AlreadyDie;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("현재는 이미 죽음");
        agent.navMeshAgent.acceleration = 0f;
        agent.navMeshAgent.destination = agent.transform.position;
        agent.navMeshAgent.speed = 0f;
        agent.navMeshAgent.velocity = Vector3.zero;

    }

    public void AIUpdate(AIAgent agent)
    {

    }



    public void Exit(AIAgent agent)
    {

    }


}
