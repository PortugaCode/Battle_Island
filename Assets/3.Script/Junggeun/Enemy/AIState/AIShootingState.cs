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
    int a;
    private EnemyHealth enemyHealth;
    private bool nowReload = false;





    public AiStateID GetID()
    {
        return AiStateID.Shooting;
    }

    public void Enter(AIAgent agent)
    {
        nowReload = false;
        a = UnityEngine.Random.Range(0, 3);
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

        if(enemyHealth.currentHealth < 50f && agent.isRun && a == 1)
        {
            agent.isRun = false;
            agent.stateMachine.ChangeState(AiStateID.RuntoWall);
            return;
        }

        if (!agent.isRun)
        {
            agent.transform.LookAt(agent.playerTarget);
            agent.AimTarget.position = Vector3.Lerp(agent.AimTarget.position, agent.playerTarget.position, 3f * Time.deltaTime);
            if (Physics.Raycast(agent.CurrentGun_Gun.muzzleTransform.position, agent.CurrentGun_Gun.muzzleTransform.forward, out RaycastHit hit, 20f))
            {
                if (hit.collider.CompareTag("Wall"))
                {
                    Debug.DrawRay(agent.CurrentGun_Gun.muzzleTransform.position, agent.CurrentGun_Gun.muzzleTransform.forward * hit.distance, Color.blue);
                }
                else if(!agent.isneedReload)
                {
                    if (agent.magAmmo <= 0)
                    {
                        if (agent.ammoRemain <= 0)
                        {
                            return;
                        }
                        else
                        {
                            animator.SetTrigger("Reload");
                            agent.isAmmoReady = true;
                            agent.isneedReload = true;
                            return;
                        }
                    }
                    Debug.DrawRay(agent.CurrentGun_Gun.muzzleTransform.position, agent.CurrentGun_Gun.muzzleTransform.forward * 1000f, Color.red);
                    //ÃÑ ½ò ¶§ Åº ·£´ýÀ¸·Î Æ¢°Ô ÇÏ±â À§ÇÑ º¯¼ö
                    x = UnityEngine.Random.Range(-2f, 2f);
                    y = UnityEngine.Random.Range(-2f, 2f);
                    if (agent.navMeshAgent.speed <= 0)
                    {
                        Physics.Raycast(agent.CurrentGun_Gun.muzzleTransform.position, agent.CurrentGun_Gun.muzzleTransform.forward, out agent.hit, Vector3.Distance(agent.CurrentGun_Gun.muzzleTransform.position, agent.playerTarget.position));
                        Debug.DrawRay(agent.CurrentGun_Gun.muzzleTransform.position, agent.CurrentGun_Gun.muzzleTransform.forward * agent.hit.distance, Color.green);

                        agent.AimTarget.position = Vector3.Lerp(agent.AimTarget.position, agent.playerTarget.position + new Vector3(x, y, 0f), 2.5f * Time.deltaTime);
                        if (agent.magAmmo > 0)
                        {
                            agent.CurrentGun_Gun.EnemyShoot(agent);
                        }
                    }
                }
                return;
            }
            return;
        }



        if (agent.navMeshAgent.speed <= 0 && agent.isRun)
        {
            x = UnityEngine.Random.Range(-2f, 2f);
            y = UnityEngine.Random.Range(-2f, 2f);
            Physics.Raycast(agent.CurrentGun_Gun.muzzleTransform.position, agent.CurrentGun_Gun.muzzleTransform.forward, out agent.hit, Vector3.Distance(agent.CurrentGun_Gun.muzzleTransform.position, agent.playerTarget.position));
            Debug.DrawRay(agent.CurrentGun_Gun.muzzleTransform.position, agent.CurrentGun_Gun.muzzleTransform.forward * agent.hit.distance, Color.green);

            agent.AimTarget.position = Vector3.Lerp(agent.AimTarget.position, agent.playerTarget.position + new Vector3(x, y, 0f), 3f * Time.deltaTime);
            if (agent.magAmmo > 0)
            {
                agent.CurrentGun_Gun.EnemyShoot(agent);
            }
            else if(agent.magAmmo <= 0 && !nowReload)
            {
                nowReload = true;
                agent.stateMachine.ChangeState(AiStateID.Reload);
            }
        }




        CheckAll(agent);
    }

    public void Exit(AIAgent agent)
    {

    }


    private void CheckAll(AIAgent agent)
    {

        Vector3 Playerdirection2 = agent.playerTarget.position - agent.transform.position;

        Vector3 agnetDirection = agent.transform.forward;
        Playerdirection2.Normalize();
        float dotProduct = Vector3.Dot(Playerdirection2, agnetDirection);

        if (dotProduct < 0.0f)
        {
            agent.stateMachine.ChangeState(AiStateID.ChasePlayer);
            return;
        }


        if (Physics.CheckSphere(agent.transform.position, 7f, agent.PlayerLayer))
        {
            Vector3 direction = agent.playerTarget.position - agent.transform.position;

            if (Physics.Raycast(agent.transform.position, direction, Vector3.Distance(agent.transform.position, agent.playerTarget.position), agent.WallLayer))
            {
                agent.stateMachine.ChangeState(AiStateID.ChasePlayer);
                return;
            }
            else
            {
                return;
            }
        }

        if (Physics.Raycast(agent.CurrentGun_Gun.muzzleTransform.position, agent.CurrentGun_Gun.muzzleTransform.forward, out RaycastHit hit, Vector3.Distance(agent.CurrentGun_Gun.muzzleTransform.position, agent.playerTarget.position)))
        {
            if(hit.collider.CompareTag("Wall"))
            {
                agent.stateMachine.ChangeState(AiStateID.ChasePlayer);
                Debug.DrawRay(agent.CurrentGun_Gun.muzzleTransform.position, agent.CurrentGun_Gun.muzzleTransform.forward * hit.distance, Color.blue);
            }
            else if (hit.collider.CompareTag("Player"))
            {
                Debug.DrawRay(agent.CurrentGun_Gun.muzzleTransform.position, agent.CurrentGun_Gun.muzzleTransform.forward * hit.distance, Color.red);
            }
            else if(hit.collider)
            {
                agent.stateMachine.ChangeState(AiStateID.ChasePlayer);
            }
            return;
        }

        Vector3 Playerdirection = agent.playerTarget.position - agent.transform.position;
        if (Playerdirection.magnitude > agent.config.maxSightDistance + 25f)
        {
            agent.stateMachine.ChangeState(AiStateID.ChasePlayer);
            return;
        }


    }



    private void CheckWall2(AIAgent agent)
    {
        if(Physics.CheckSphere(agent.CurrentGun_Gun.muzzleTransform.position, 0.5f, agent.WallLayer))
        {
            agent.stateMachine.ChangeState(AiStateID.ChasePlayer);
        }
    }

    private void CheckPlayer(AIAgent agent)
    {
        Vector3 Playerdirection = agent.playerTarget.position - agent.transform.position;
        if (Playerdirection.magnitude > agent.config.maxSightDistance+40f)
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

/*    private void Fire(AIAgent agent)
    {
        if(Time.time >= lastFireTime + agent.Nowgundata.timebetFire)
        {
            lastFireTime = Time.time;

            Shot(agent);
            agent.isShot = true;
        }
    }*/

    /*private void Shot(AIAgent agent)
    {
        
        Debug.Log("¹ß»ç");

        GameObject b = BulletPooling.Instance.Bullets.Dequeue();
        b.gameObject.SetActive(true);
        b.transform.position = agent.SelectStartAim.position;
        b.transform.rotation = agent.SelectStartAim.rotation;


        agent.FireEffect.transform.position = agent.SelectStartAim.position;
        agent.FireEffect1.transform.position = agent.SelectRifleWeapons.transform.position;
        agent.FireEffect.Play();
        agent.FireEffect1.Play();
        animator.SetTrigger("Fire");
        agent.enemyAudio.PlayShot();
        agent.FireEffect2.transform.position = agent.hit.point;

        if (agent.hit.collider.CompareTag("Wall"))
        {
            agent.FireEffect2.Play();
        }

        agent.magAmmo--;


        //GameObject b = MonoBehaviour.Instantiate(agent.Bullet, agent.SelectStartAim.position, agent.SelectStartAim.transform.rotation);


        *//*        Vector3 direction = b.transform.position - agent.AimTarget.position;
                direction.Normalize();
                b.transform.forward = direction;
        
                 GameObject light = MonoBehaviour.Instantiate(agent.FireLight, agent.SelectStartAim.position, Quaternion.identity);
        MonoBehaviour.Destroy(light, 0.03f);*//*
    }*/


}
