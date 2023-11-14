using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIRandomMoveState : AIState
{
    private Vector3 point;
    private Vector3 distance;
    private DeadZone deadZone;

    private float rid;
    private Vector3 center;

    public AiStateID GetID()
    {
        return AiStateID.RandomMove;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("랜덤 이동");
        GameObject.FindGameObjectWithTag("DeadZone").TryGetComponent(out deadZone);
        point = GetRandomPoint(new Vector3(16, 0, -31), 60f);
        agent.navMeshAgent.destination = point;
        agent.navMeshAgent.speed = 5;
        Debug.DrawRay(point, Vector3.up * 5000f, Color.green, 10f);
    }

    public void AIUpdate(AIAgent agent)
    {
        rid = deadZone.CurrentRadius();
        center = deadZone.CurrentDeadZonePosition();

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

        distance = agent.transform.position - point;
        if (distance.magnitude <= 5f)
        {
            //point = null; //나중에 Vector 000으로 바꾸기
            point = GetRandomPoint(center, rid);
            agent.navMeshAgent.destination = point;
            agent.navMeshAgent.speed = 5;
            Debug.DrawRay(point, Vector3.up * 5000f, Color.green, 10f);
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

    /*    private GameObject FindClosestPoint(AIAgent agnet)
        {
            GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
            int i = Random.Range(0, points.Length);

            GameObject point = points[i];
            return point;
        }*/

    private Vector3 GetRandomPoint(Vector3 center, float MaxDistance)
    {
        Vector3 randomPos = Random.insideUnitSphere * MaxDistance + center;
        randomPos.y = 3f;
        NavMeshHit hit;

        NavMesh.SamplePosition(randomPos, out hit, MaxDistance, NavMesh.AllAreas);
        #region
        /*        while (true)
                {
                    NavMesh.SamplePosition(randomPos, out hit, MaxDistance, NavMesh.AllAreas);
                    if (hit.position.y < 3f)
                    {
                        break;
                    }
                }*/
        #endregion
        return hit.position;
    }

}
