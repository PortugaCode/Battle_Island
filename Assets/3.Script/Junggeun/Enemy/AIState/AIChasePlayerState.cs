using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChasePlayerState : AIState
{

    int a;
    private float timer = 0.0f;
    private EnemyHealth enemyHealth;

    public AiStateID GetID()
    {
        return AiStateID.ChasePlayer;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("쫓기");
        enemyHealth = agent.gameObject.GetComponent<EnemyHealth>();
        agent.navMeshAgent.speed = 3f;
        a = UnityEngine.Random.Range(0, 3);
    }

    public void AIUpdate(AIAgent agent)
    {
        if (agent.enemyHealth.IsDie)
        {
            agent.navMeshAgent.velocity = Vector3.zero;
            return;
        }

        agent.AimTarget.position = Vector3.Lerp(agent.AimTarget.position, agent.originTarget.position, 5f * Time.deltaTime);

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


        if (Physics.CheckSphere(agent.transform.position, 5f, agent.PlayerLayer))
        {
            agent.stateMachine.ChangeState(AiStateID.Shooting);
            return;
        }


        if (CheckWall(agent) || CheckWall2(agent) || CheckWall3(agent)) return;


        Vector3 Playerdirection = agent.playerTarget.position - agent.transform.position;
        if (Playerdirection.magnitude > agent.config.maxSightDistance+40f)
        {
            return;
        }

        Vector3 agnetDirection = agent.transform.forward;
        Playerdirection.Normalize();
        float dotProduct = Vector3.Dot(Playerdirection, agnetDirection);

        if (dotProduct > 0.5f)
        {
            agent.stateMachine.ChangeState(AiStateID.Shooting);
            return;
        }


        if (enemyHealth.currentHealth < 50f && a == 1 && agent.isRun)
        {
            agent.isRun = false;
            agent.stateMachine.ChangeState(AiStateID.RuntoWall);
            return;
        }
    }

    public void Exit(AIAgent agent)
    {
        
    }

    private bool CheckWall(AIAgent agent)
    {
        if (Physics.Raycast(agent.SelectStartAim.transform.position, agent.SelectStartAim.transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                Debug.DrawRay(agent.SelectStartAim.transform.position, agent.SelectStartAim.transform.forward * hit.distance, Color.blue);
                return true;
            }
            else
            {
                Debug.DrawRay(agent.SelectStartAim.transform.position, agent.SelectStartAim.transform.forward * hit.distance, Color.red);
                return false;
            }
        }
        return true;
    }

    private bool CheckWall2(AIAgent agent)
    {
        if (Physics.CheckSphere(agent.SelectStartAim.position, 0.5f, agent.WallLayer))
        {
            return true;
        }
        return false;
    }

    private bool CheckWall3(AIAgent agent)
    {
        if (Physics.CheckSphere(agent.transform.position, 1.5f, agent.WallLayer))
        {
            return true;
        }
        return false;
    }
}
