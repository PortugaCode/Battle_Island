using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShootingState : AIState
{
    private float lastFireTime;
    private Animator animator;
    private float x;
    private float y;

    private EnemyHealth enemyHealth;

    private bool isgotowall = true;




    public AiStateID GetID()
    {
        return AiStateID.Shooting;
    }

    public void Enter(AIAgent agent)
    {
        animator = agent.gameObject.GetComponent<Animator>();
        enemyHealth = agent.gameObject.GetComponent<EnemyHealth>();
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

        if(enemyHealth.currentHealth < 50f && isgotowall)
        {
            isgotowall = false;
            agent.stateMachine.ChangeState(AiStateID.RuntoWall);
        }
        CheckWall(agent);
        CheckPlayer(agent);
        CheckPlayer2(agent);


        //ÃÑ ½ò ¶§ Åº ·£´ýÀ¸·Î Æ¢°Ô ÇÏ±â À§ÇÑ º¯¼ö
        x = UnityEngine.Random.Range(-3f, 3f);
        y = UnityEngine.Random.Range(-3f, 3f);


        Physics.Raycast(agent.SelectStartAim.position, agent.SelectStartAim.forward, out agent.hit, Mathf.Infinity);
        Debug.DrawRay(agent.SelectStartAim.position, agent.SelectStartAim.forward * 1000f, Color.green);

        agent.AimTarget.position = Vector3.Lerp(agent.AimTarget.position, agent.playerTarget.position + new Vector3(x, y, 0f), 4f * Time.deltaTime);

        if(agent.navMeshAgent.speed <= 0)
        {
            if (agent.magAmmo > 0)
            {
                Fire(agent);
            }
            else if(agent.magAmmo <= 0)
            {
                agent.stateMachine.ChangeState(AiStateID.Reload);
            }

        }



    }

    public void Exit(AIAgent agent)
    {

    }


    private void CheckWall(AIAgent agent)
    {
        if(Physics.Raycast(agent.SelectStartAim.transform.position, agent.SelectStartAim.transform.forward, out RaycastHit hit, 20f))
        {
            if(hit.collider.CompareTag("Wall"))
            {
                agent.stateMachine.ChangeState(AiStateID.ChasePlayer);
                Debug.DrawRay(agent.SelectStartAim.transform.position, agent.SelectStartAim.transform.forward * hit.distance, Color.blue);
            }
            else
            {
                Debug.DrawRay(agent.SelectStartAim.transform.position, agent.SelectStartAim.transform.forward * 1000f, Color.red);
            }
        }
    }

    private void CheckWall2(AIAgent agent)
    {
        if(Physics.CheckSphere(agent.SelectStartAim.position, 0.5f, agent.WallLayer))
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


        GameObject b = MonoBehaviour.Instantiate(agent.Bullet, agent.SelectStartAim.position, agent.SelectStartAim.transform.rotation);
        GameObject light = MonoBehaviour.Instantiate(agent.FireLight, agent.SelectStartAim.position, Quaternion.identity);
        MonoBehaviour.Destroy(light, 0.03f);
        agent.FireEffect.transform.position = agent.SelectStartAim.position;
        agent.FireEffect1.transform.position = agent.SelectRifleWeapons.transform.position;
        agent.FireEffect.Play();
        agent.FireEffect1.Play();
        animator.SetTrigger("Fire");
        agent.magAmmo--;
        /*        Vector3 direction = b.transform.position - agent.AimTarget.position;
                direction.Normalize();
                b.transform.forward = direction;*/
    }


}
