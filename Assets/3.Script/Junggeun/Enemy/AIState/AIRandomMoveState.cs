using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRandomMoveState : AIState
{
    private GameObject point;
    private Vector3 distance;


    public AiStateID GetID()
    {
        return AiStateID.RandomMove;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("·£´ý ÀÌµ¿");
        point = FindClosestPoint(agent);
        agent.navMeshAgent.destination = point.transform.position;
        agent.navMeshAgent.speed = 5;
    }

    public void AIUpdate(AIAgent agent)
    {
        agent.AimTarget.position = Vector3.Lerp(agent.AimTarget.position, agent.originTarget.position, 2f * Time.deltaTime);

        if (FindPlayer(agent) && agent.isReady && agent.isAmmoReady)
        {
            agent.stateMachine.ChangeState(AiStateID.ChasePlayer);
            return;
        }

        if (FindWeapon(agent) && !agent.isReady)
        {
            agent.stateMachine.ChangeState(AiStateID.FindWeapon);
            return;
        }

        if (FindBullet(agent))
        {
            agent.stateMachine.ChangeState(AiStateID.FindBullet);
            return;
        }

        if (FindArmor(agent) && !agent.isArmor)
        {
            agent.stateMachine.ChangeState(AiStateID.FindArmor);
            return;
        }


        if (FindPlayer(agent))
        {
            agent.navMeshAgent.speed = 5.5f;
        }
        else
        {
            agent.navMeshAgent.speed = 5f;
        }

        distance = agent.transform.position - point.transform.position;
        if (distance.magnitude <= 0.5f)
        {
            point = null;
            point = FindClosestPoint(agent);
            agent.navMeshAgent.destination = point.transform.position;
            agent.navMeshAgent.speed = 5;
        }
    }

    public void Exit(AIAgent agent)
    {

    }


    private bool FindPlayer(AIAgent agent)
    {
        Collider[] w = Physics.OverlapSphere(agent.transform.position, 20f);
        foreach (Collider col in w)
        {
            if (col.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    private bool FindWeapon(AIAgent agent)
    {
        Collider[] w = Physics.OverlapSphere(agent.transform.position, 20f);
        foreach(Collider col in w)
        {
            if(col.CompareTag("Weapon"))
            {
                return true;
            }
        }
        return false;
    }

    private bool FindBullet(AIAgent agent)
    {
        Collider[] w = Physics.OverlapSphere(agent.transform.position, 20f);
        foreach (Collider col in w)
        {
            if (col.CompareTag("Bullet"))
            {
                return true;
            }
        }
        return false;
    }

    private bool FindArmor(AIAgent agent)
    {
        Collider[] w = Physics.OverlapSphere(agent.transform.position, 20f);
        foreach (Collider col in w)
        {
            if (col.CompareTag("Armor"))
            {
                return true;
            }
        }
        return false;
    }

    private GameObject FindClosestPoint(AIAgent agnet)
    {
        GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
        int i = Random.Range(0, points.Length);
        
        GameObject point = points[i];
        return point;
    }

}
