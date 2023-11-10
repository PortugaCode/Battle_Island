using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicChasePlayerState : HelicState
{
    private Transform point;
    private Vector3 distance;
    private Vector3 direction;
    private float rotationspeed = 1f;

    public HelicStateID GetID()
    {
        return HelicStateID.ChasePlayer;
    }

    public void Enter(HelicAgent agent)
    {
        point = agent.Player;
    }

    public void AIUpdate(HelicAgent agent)
    {
        Vector3 Playerdirection = agent.Player.position - agent.hit.point;
        if (Playerdirection.magnitude > 20f)
        {
            MovetoPlayer(agent);
            return;
        }
        Vector3 agnetDirection = agent.BodyTarget.forward;
        Playerdirection.Normalize();
        float dotProduct = Vector3.Dot(Playerdirection, agnetDirection);

        if(dotProduct > 0.0f)
        {
            agent.stateMachine.ChangeState(HelicStateID.Shooting);
        }
        else if (dotProduct < 0.0f)
        {
            MovetoPlayer(agent);
        }





    }

    public void Exit(HelicAgent agent)
    {

    }


    private void MovetoPlayer(HelicAgent agent)
    {
        direction = point.transform.position - agent.PositionTarget.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        agent.BodyTarget.rotation = Quaternion.Lerp(agent.BodyTarget.rotation, rotation, rotationspeed * Time.deltaTime);

        agent.PositionTarget.Translate(agent.BodyTarget.forward * 3f * Time.deltaTime);

        if (agent.PositionTarget.position.y <= 13f)
        {
            agent.PositionTarget.Translate(agent.BodyTarget.up * 3f * Time.deltaTime);
        }
    }


    private void WallCheck(HelicAgent agent)
    {

    }

}
