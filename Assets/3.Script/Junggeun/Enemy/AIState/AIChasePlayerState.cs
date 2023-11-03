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
        Debug.Log("�ѱ�");

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
            if (sqdistance > agent.config.maxDistance * agent.config.maxDistance) // ������ �ϴ� ������ SqrMagitude�� ������ �� ���� ��ȯ�ϱ� ������
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

        if (dotProduct > 0.0f)
        {
            agent.stateMachine.ChangeState(AiStateID.Shooting);
        }
    }

    public void Exit(AIAgent agent)
    {
        
    }

    private bool CheckWall(AIAgent agent)
    {
        if (Physics.Raycast(agent.StartAim[2].transform.position, agent.StartAim[2].transform.forward, out RaycastHit hit, 10f))
        {
            if (hit.collider.CompareTag("Finish"))
            {
                
                Debug.DrawRay(agent.StartAim[2].transform.position, agent.StartAim[2].transform.forward * hit.distance, Color.blue);
                return true;
            }
            else
            {
                
                Debug.DrawRay(agent.StartAim[2].transform.position, agent.StartAim[2].transform.forward * 1000f, Color.red);
                return false;
            }
        }
        return false;
    }

    private bool CheckWall2(AIAgent agent)
    {
        if (Physics.CheckSphere(agent.StartAim[2].position, 2f, agent.WallLayer))
        {
            return true;
        }
        return false;
    }
}
