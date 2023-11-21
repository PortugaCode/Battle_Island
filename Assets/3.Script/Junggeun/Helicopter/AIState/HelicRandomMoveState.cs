using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HelicRandomMoveState : HelicState
{
    private GameObject point;
    private Vector3 distance;
    private Vector3 direction;
    private Vector3 direction2;

    private float rotationspeed = 0.4f;


    public HelicStateID GetID()
    {
        return HelicStateID.RandomMove;
    }


    public void Enter(HelicAgent agent)
    {
        point = GetRandomPoint();
    }

    public void AIUpdate(HelicAgent agent)
    {
        Move(agent);
        direction2 = agent.Player.position - agent.PositionTarget.transform.position;
        Debug.DrawRay(agent.PositionTarget.position, direction2 * 1000f, Color.blue);


        if (FindPlayer(agent))
        {
            agent.stateMachine.ChangeState(HelicStateID.ChasePlayer);
            return;
        }


        distance = agent.PositionTarget.position - point.transform.position;
        if (distance.magnitude <= 8f)
        {
            point = GetRandomPoint();
        }

    }

    public void Exit(HelicAgent agent)
    {

    }

    private void Move(HelicAgent agent)
    {
        direction = point.transform.position - agent.PositionTarget.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        agent.BodyTarget.rotation = Quaternion.Lerp(agent.BodyTarget.rotation, rotation, rotationspeed * Time.deltaTime);

        agent.PositionTarget.Translate(agent.BodyTarget.forward * 10f * Time.deltaTime);
    }

    private bool FindPlayer(HelicAgent agent)
    {
        Collider[] w = Physics.OverlapSphere(agent.hit.point, 15f);
        foreach (Collider col in w)
        {
            if (col.CompareTag("Player"))
            {
                if(Physics.Raycast(agent.PositionTarget.position, direction2, Vector3.Distance(agent.PositionTarget.position, agent.Player.position), agent.WallLayer))
                {
                    return false;
                }
                else { return true; }
            }
        }
        return false;
    }







    private GameObject GetRandomPoint()
    {
        GameObject[] a = GameObject.FindGameObjectsWithTag("Finish");
        int index = Random.Range(0, 7);

        return a[index];
    }


}
