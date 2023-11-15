using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicShootingState : HelicState
{
    private float lastFireTime;
    private Vector3 direction;
    private float rotationspeed = 5f;


    public HelicStateID GetID()
    {
        return HelicStateID.Shooting;
    }

    public void Enter(HelicAgent agent)
    {

    }

    public void AIUpdate(HelicAgent agent)
    {
        if (agent.PositionTarget.position.y <= 13f)
        {
            agent.PositionTarget.Translate(agent.BodyTarget.up * 5f * Time.deltaTime);
        }


        direction = agent.Player.transform.position - agent.PositionTarget.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        agent.BodyTarget.rotation = Quaternion.Lerp(agent.BodyTarget.rotation, rotation, rotationspeed * Time.deltaTime);
        agent.AimTarget.position = Vector3.Lerp(agent.AimTarget.position, agent.Player.transform.position, 5f * Time.deltaTime);


        Vector3 Playerdirection = agent.Player.position - agent.hit.point;
        if (Playerdirection.magnitude > 20f)
        {
            agent.stateMachine.ChangeState(HelicStateID.ChasePlayer);
            return;
        }

        Vector3 agnetDirection = agent.BodyTarget.forward;
        Playerdirection.Normalize();
        float dotProduct = Vector3.Dot(Playerdirection, agnetDirection);

        if (dotProduct > 0.0f)
        {
            Fire(agent);
        }

        else if(dotProduct < 0.0f)
        {
            agent.stateMachine.ChangeState(HelicStateID.ChasePlayer);
        }
    }

    public void Exit(HelicAgent agent)
    {

    }


    private void Fire(HelicAgent agent)
    {
        if (Time.time >= lastFireTime + 0.2f)
        {
            lastFireTime = Time.time;

            Shot(agent);
            agent.isShot = true;
        }
    }

    private void Shot(HelicAgent agent)
    {
        GameObject b = BulletPooling.Instance.Bullets.Dequeue();
        b.gameObject.SetActive(true);
        b.transform.position = agent.OriginalTarget.position;
        b.transform.rotation = agent.OriginalTarget.rotation;
        agent.FireEffect.transform.position = agent.OriginalTarget.position;
        agent.FireEffect1.transform.position = agent.OriginalTarget.transform.position;
        agent.FireEffect.Play();
        agent.FireEffect1.Play();
    }


}
