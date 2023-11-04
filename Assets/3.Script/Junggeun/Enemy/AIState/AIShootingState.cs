using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShootingState : AIState
{
    private float lastFireTime;
    private Animator animator;




    public AiStateID GetID()
    {
        return AiStateID.Shooting;
    }

    public void Enter(AIAgent agent)
    {
        animator = agent.gameObject.GetComponent<Animator>();
        Debug.Log("½´ÆÃ");
        agent.twoBoneIK.weight = 1f;
        agent.rig.weight = 1f;
        while (agent.navMeshAgent.speed != 0)
        {
            agent.navMeshAgent.speed -= 0.1f;
        }
    }

    public void AIUpdate(AIAgent agent)
    {
        if (agent.enemyHealth.IsDie)
        {
            agent.navMeshAgent.velocity = Vector3.zero;
            return;
        }

        Physics.Raycast(agent.StartAim[2].position, agent.StartAim[2].forward, out agent.hit, Mathf.Infinity);
        Debug.DrawRay(agent.StartAim[2].position, agent.StartAim[2].forward * 1000f, Color.green);

        agent.AimTarget.position = Vector3.Lerp(agent.AimTarget.position, agent.playerTarget.position, 5f * Time.deltaTime);
        if(agent.navMeshAgent.speed <= 0)
        {
            if(agent.magAmmo > 0)
            {
                Fire(agent);
            }
            else if(agent.magAmmo <= 0)
            {
                agent.stateMachine.ChangeState(AiStateID.Reload);
            }
            CheckWall(agent);


            CheckPlayer(agent);
            CheckPlayer2(agent);
        }



    }

    public void Exit(AIAgent agent)
    {

    }


    private void CheckWall(AIAgent agent)
    {
        if(Physics.Raycast(agent.StartAim[2].transform.position, agent.StartAim[2].transform.forward, out RaycastHit hit, 7f))
        {
            if(hit.collider.CompareTag("Finish"))
            {
                agent.stateMachine.ChangeState(AiStateID.ChasePlayer);
                Debug.DrawRay(agent.StartAim[2].transform.position, agent.StartAim[2].transform.forward * hit.distance, Color.blue);
            }
            else
            {
                Debug.DrawRay(agent.StartAim[2].transform.position, agent.StartAim[2].transform.forward * 1000f, Color.red);
            }
        }
    }

    private void CheckWall2(AIAgent agent)
    {
        if(Physics.CheckSphere(agent.StartAim[2].position, 1f, agent.WallLayer))
        {
            agent.stateMachine.ChangeState(AiStateID.ChasePlayer);
        }
    }

    private void CheckPlayer(AIAgent agent)
    {
        Vector3 Playerdirection = agent.playerTarget.position - agent.transform.position;
        if (Playerdirection.magnitude > agent.config.maxSightDistance+15f)
        {
            agent.stateMachine.ChangeState(AiStateID.ChasePlayer);
        }
    }

    private void CheckPlayer2(AIAgent agent)
    {
        Vector3 Playerdirection2 = agent.playerTarget.position - agent.transform.position;

        Vector3 agnetDirection = agent.transform.forward;
        Playerdirection2.Normalize();
        float dotProduct = Vector3.Dot(Playerdirection2, agnetDirection);

        if (dotProduct < 0.0f)
        {
            agent.stateMachine.ChangeState(AiStateID.ChasePlayer);
        }
    }

    private void Fire(AIAgent agent)
    {
        if(Time.time >= lastFireTime + agent.Nowgundata.timebetFire)
        {
            lastFireTime = Time.time;

            Shot(agent);
            agent.isShot = true;
        }
    }

    private void Shot(AIAgent agent)
    {
        
        Debug.Log("¹ß»ç");
        GameObject b = MonoBehaviour.Instantiate(agent.Bullet, agent.StartAim[2].position, agent.StartAim[2].transform.rotation);
        GameObject light = MonoBehaviour.Instantiate(agent.FireLight, agent.StartAim[2].position, Quaternion.identity);
        MonoBehaviour.Destroy(light, 0.03f);
        agent.FireEffect.transform.position = agent.StartAim[2].position;
        agent.FireEffect1.transform.position = agent.rifleWeapons[2].transform.position;
        agent.FireEffect.Play();
        agent.FireEffect1.Play();
        animator.SetTrigger("Fire");
        agent.magAmmo--;
        /*        Vector3 direction = b.transform.position - agent.AimTarget.position;
                direction.Normalize();
                b.transform.forward = direction;*/
    }


}
