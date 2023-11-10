using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HelicRandomMoveState : HelicState
{
    private GameObject point;
    private Vector3 distance;
    private Vector3 direction;

    private float rotationspeed = 0.4f;

    private Rigidbody rigidbody;

    public HelicStateID GetID()
    {
        return HelicStateID.RandomMove;
    }


    public void Enter(HelicAgent agent)
    {
        point = GetRandomPoint();
        agent.gameObject.TryGetComponent(out rigidbody);

    }

    public void AIUpdate(HelicAgent agent)
    {
        direction = point.transform.position - agent.PositionTarget.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        agent.BodyTarget.rotation = Quaternion.Lerp(agent.BodyTarget.rotation, rotation, rotationspeed * Time.deltaTime);

        agent.PositionTarget.Translate(agent.BodyTarget.forward * 10f * Time.deltaTime);




        distance = agent.PositionTarget.position - point.transform.position;
        if (distance.magnitude <= 5f)
        {
            point = GetRandomPoint();
        }

    }

    public void Exit(HelicAgent agent)
    {

    }








    private GameObject GetRandomPoint()
    {
        GameObject[] a = GameObject.FindGameObjectsWithTag("Finish");
        int index = Random.Range(0, 4);

        return a[index];
    }


}
