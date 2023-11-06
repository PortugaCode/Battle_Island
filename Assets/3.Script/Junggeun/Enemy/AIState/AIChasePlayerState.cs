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
        Debug.Log("쫓기");

        agent.navMeshAgent.speed = 3f;
    }

    public void AIUpdate(AIAgent agent)
    {
        if (agent.enemyHealth.IsDie)
        {
            agent.navMeshAgent.velocity = Vector3.zero;
            return;
        }

        agent.AimTarget.position = Vector3.Lerp(agent.AimTarget.position, agent.originTarget.position, 2f * Time.deltaTime);



        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            float sqdistance = (agent.playerTarget.position - agent.navMeshAgent.destination).sqrMagnitude;
            if (sqdistance > agent.config.maxDistance * agent.config.maxDistance) // 제곱을 하는 이유는 SqrMagitude는 제곱을 한 값을 반환하기 때문에
            {
                agent.navMeshAgent.destination = agent.playerTarget.position;
            }
            timer = agent.config.maxTime;
        }

        if(CheckWall(agent) || CheckWall2(agent)) return;


        Vector3 Playerdirection = agent.playerTarget.position - agent.transform.position;
        if (Playerdirection.magnitude > agent.config.maxSightDistance+15f)
        {
            return;
        }

        Vector3 agnetDirection = agent.transform.forward;
        Playerdirection.Normalize();
        float dotProduct = Vector3.Dot(Playerdirection, agnetDirection);

        if (dotProduct > 0.5f)
        {
            agent.stateMachine.ChangeState(AiStateID.Shooting);
        }
    }

    public void Exit(AIAgent agent)
    {
        
    }

    private bool CheckWall(AIAgent agent)
    {
        if (Physics.Raycast(agent.SelectStartAim.transform.position, agent.SelectStartAim.transform.forward, out RaycastHit hit, 10f))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                Debug.DrawRay(agent.SelectStartAim.transform.position, agent.SelectStartAim.transform.forward * hit.distance, Color.blue);
                return true;
            }
            else
            {
                
                Debug.DrawRay(agent.SelectStartAim.transform.position, agent.SelectStartAim.transform.forward * 1000f, Color.red);
                return false;
            }
        }
        return false;
    }

    private bool CheckWall2(AIAgent agent)
    {
        if (Physics.CheckSphere(agent.SelectStartAim.position, 1.5f, agent.WallLayer))
        {
            return true;
        }
        return false;
    }
}
