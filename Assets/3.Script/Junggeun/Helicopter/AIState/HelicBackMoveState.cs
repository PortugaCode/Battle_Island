using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicBackMoveState : HelicState
{
    Vector3 direction;
    Vector3 Godirection;
    private float rotationspeed = 1f;
    float x;

    public HelicStateID GetID()
    {
        return HelicStateID.BackMove;
    }

    public void Enter(HelicAgent agent)
    {

    }

    public void AIUpdate(HelicAgent agent)
    {
        UpAngle(agent);
        GoBackMove(agent);

        Vector3 Playerdirection = agent.Player.position - agent.hit.point;
        if (Playerdirection.magnitude < 15f)
        {
            return;
        }
        Vector3 agnetDirection = agent.BodyTarget.forward;
        Playerdirection.Normalize();
        float dotProduct = Vector3.Dot(Playerdirection, agnetDirection);

        if (dotProduct > 0.0f)
        {
            agent.stateMachine.ChangeState(HelicStateID.Shooting);
        }
    }

    private void UpAngle(HelicAgent agent)
    {
        direction = agent.Player.transform.position + new Vector3(0, 20f, 0f) - agent.PositionTarget.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        agent.BodyTarget.rotation = Quaternion.Lerp(agent.BodyTarget.rotation, rotation, rotationspeed * Time.deltaTime);
        agent.AimTarget.position = Vector3.Lerp(agent.AimTarget.position, agent.OriginalAimTarget.position, rotationspeed * Time.deltaTime);
    }

    private void GoBackMove(HelicAgent agent)
    {
        Godirection = (agent.hit.point - agent.Player.transform.position).normalized;
        agent.PositionTarget.Translate(Godirection * 4f * Time.deltaTime);

        if (agent.PositionTarget.position.y <= 20f)
        {
            agent.PositionTarget.Translate(agent.BodyTarget.up * 4f * Time.deltaTime);
        }
    }

    public void Exit(HelicAgent agent)
    {

    }


}
