using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicShootingState : HelicState
{
    private float lastFireTime;
    private Vector3 direction;
    private float rotationspeed = 4f;

    float x;
    float y;


    public HelicStateID GetID()
    {
        return HelicStateID.Shooting;
    }

    public void Enter(HelicAgent agent)
    {

    }

    public void AIUpdate(HelicAgent agent)
    {
        FollowPlayer(agent);

        ShotorChase(agent);

    }

    public void Exit(HelicAgent agent)
    {

    }

    private void FollowPlayer(HelicAgent agent)
    {
        direction = agent.Player.transform.position - agent.PositionTarget.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        agent.BodyTarget.rotation = Quaternion.Lerp(agent.BodyTarget.rotation, rotation, rotationspeed * Time.deltaTime);
        agent.AimTarget.position = Vector3.Lerp(agent.AimTarget.position, agent.Player.transform.position, rotationspeed * Time.deltaTime);
    }

    private void ShotorChase(HelicAgent agent)
    {
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

        else if (dotProduct < 0.0f)
        {
            agent.stateMachine.ChangeState(HelicStateID.ChasePlayer);
        }
    }


    private void Fire(HelicAgent agent)
    {

        x = UnityEngine.Random.Range(-0.3f, 0.3f);
        y = UnityEngine.Random.Range(-0.3f, 0.3f);
        if (Time.time >= lastFireTime + 0.1f)
        {
            lastFireTime = Time.time;

            Shot(agent);
            agent.isShot = true;
        }
    }

    private void Shot(HelicAgent agent)
    {
        GameObject b = BulletPooling.Instance.Bullets.Dequeue();

        b.transform.position = agent.OriginalTarget.position + new Vector3(x, y, 0);
        b.transform.rotation = agent.OriginalTarget.rotation;
        b.gameObject.SetActive(true);


        agent.FireEffect.transform.position = agent.OriginalTarget.position;
        agent.FireEffect1.transform.position = agent.OriginalTarget.transform.position;
        agent.FireEffect.Play();
        agent.FireEffect1.Play();
    }


}
