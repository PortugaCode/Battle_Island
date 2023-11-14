using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChasePlayerState : AIState
{

    int a;
    private float timer = 0.0f;
    private float Chasetimer = 0.0f;
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


        Chasetimer += Time.deltaTime;
        if (agent.enemyHealth.IsDie)
        {
            agent.navMeshAgent.velocity = Vector3.zero;
            return;
        }

        agent.AimTarget.position = Vector3.Lerp(agent.AimTarget.position, agent.originTarget.position, 3f * Time.deltaTime);

        if(Chasetimer >= 10f)
        {
            agent.stateMachine.ChangeState(AiStateID.RandomMove);
            Chasetimer = 0f;
            return;
        }

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

        Vector3 Playerdirection2 = agent.playerTarget.position - agent.transform.position;

        Vector3 agnetDirection2 = agent.transform.forward;
        Playerdirection2.Normalize();
        float dotProduct2 = Vector3.Dot(Playerdirection2, agnetDirection2);

        if (dotProduct2 < 0.0f)
        {
            return;
        }


        if (Physics.CheckSphere(agent.transform.position, 15f, agent.PlayerLayer))
        {
            Vector3 direction = agent.playerTarget.position - agent.transform.position ;

            if (Physics.Raycast(agent.transform.position, direction, Vector3.Distance(agent.transform.position, agent.playerTarget.position), agent.WallLayer))
            {
                return;
            }
            else
            {
                agent.stateMachine.ChangeState(AiStateID.Shooting);
                return;
            }
        }


        if (CheckWall(agent) || CheckWall2(agent) || CheckWall3(agent)) return;


        Vector3 Playerdirection = agent.playerTarget.position - agent.transform.position;
        if (Playerdirection.magnitude > agent.config.maxSightDistance+30f)
        {
            return;
        }

        Vector3 agnetDirection = agent.transform.forward;
        Playerdirection.Normalize();
        float dotProduct = Vector3.Dot(Playerdirection, agnetDirection);

        if (dotProduct > 0.0f)
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
        if (Physics.Raycast(agent.CurrentGun_Gun.muzzleTransform.position, agent.CurrentGun_Gun.muzzleTransform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                Debug.DrawRay(agent.CurrentGun_Gun.muzzleTransform.position, agent.CurrentGun_Gun.muzzleTransform.forward * hit.distance, Color.blue);
                return true;
            }
            else if(hit.collider.CompareTag("Player"))
            {
                Debug.DrawRay(agent.CurrentGun_Gun.muzzleTransform.position, agent.CurrentGun_Gun.muzzleTransform.forward * hit.distance, Color.red);
                return false;
            }
        }
        return true;
    }

    private bool CheckWall2(AIAgent agent)
    {
        if (Physics.CheckSphere(agent.CurrentGun_Gun.muzzleTransform.position, 1f, agent.WallLayer))
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
