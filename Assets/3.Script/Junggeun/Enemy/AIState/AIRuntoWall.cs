using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRuntoWall : AIState
{
    private Vector3 start;
    private Vector3 end;
    private Vector3 mid;
    private Vector3 lastMid;

    private Vector3 direction;
    private Vector3 resultPoint;
    private Vector3 distance;

    private GameObject ClosetWall;


    public AiStateID GetID()
    {
        return AiStateID.RuntoWall;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("�� ã��");
        agent.navMeshAgent.speed = 5.5f;
        ClosetWall = CheckCloseWall(agent);




        BinarySearch(agent);
        agent.navMeshAgent.destination = resultPoint;
    }

    public void AIUpdate(AIAgent agent)
    {

        agent.AimTarget.position = Vector3.Lerp(agent.AimTarget.position, agent.originTarget.position, 2f * Time.deltaTime);
        distance = agent.transform.position - resultPoint;
        if (distance.magnitude <= 1.5f)
        {
            agent.stateMachine.ChangeState(AiStateID.Standby);
            return;
        }


    }

    public void Exit(AIAgent agent)
    {
        while (agent.navMeshAgent.speed != 0)
        {
            agent.navMeshAgent.speed -= 0.1f;
        }
    }

    private void BinarySearch(AIAgent agent) //����Ž�� �˰�����
    {
        start = ClosetWall.transform.position;
        direction = ClosetWall.transform.position - agent.playerTarget.position;
        end = start += direction.normalized * 20f;

/*        if (CheckCol(agent, end))
        {
            agent.stateMachine.ChangeState(AiStateID.Shooting);
            return;
        }*/

        mid = (start + end) / 2;
        lastMid = mid;

        while(true)
        {
            if (CheckCol(agent, mid))
            {
                start = mid += direction.normalized;
            }
            else if (!CheckCol(agent, mid))
            {
                lastMid = mid;
                end = mid -= direction.normalized;
            }
            if (CheckCol(agent, mid) && !CheckCol(agent, lastMid))
            {
                resultPoint = lastMid;
                return;
            }
        }
    }

    private GameObject CheckCloseWall(AIAgent agent)
    {
        GameObject[] Walls = GameObject.FindGameObjectsWithTag("Wall");
        GameObject closestWall = null;
        float closestDistance = float.MaxValue;

        foreach (var wall in Walls)
        {
            float distanceToWall = Vector3.Distance(agent.transform.position, wall.transform.position);
            if (distanceToWall < closestDistance)
            {
                closestDistance = distanceToWall;
                closestWall = wall;
            }
        }
        return closestWall;
    }

    private bool CheckCol(AIAgent agent, Vector3 a)
    {
        if (Physics.CheckSphere(a, 1f, agent.WallLayer))
        {
            return true;
        }
        return false;
    }


    /*        Vector3 a = new Vector3(1, 1, 1);
        Vector3 b = new Vector3(4, 1, 4);

        Vector3 direction = a - b;

        direction += direction.normalized * 10f;*/

}