using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFindBulletState : MonoBehaviour, AIState
{
    private GameObject pickup;
    private bool isPickup = false;
    private Animator animator;


    public AiStateID GetID()
    {
        return AiStateID.FindBullet ;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("총알 찾기");

        animator = agent.gameObject.GetComponent<Animator>();
        pickup = FindClosestBullet(agent);
        agent.navMeshAgent.destination = pickup.transform.position;
        agent.navMeshAgent.speed = 5;
    }

    public void AIUpdate(AIAgent agent)
    {
        if (pickup == null)
        {
            agent.stateMachine.ChangeState(AiStateID.Idle);
        }

        else if (pickup.GetComponent<HaveGunCheck>().isEquip)
        {
            pickup = FindClosestBullet(agent);
            if (pickup != null)
            {
                agent.stateMachine.ChangeState(AiStateID.Idle);
            }
        }


        CheckPickup(agent);

        if (isPickup)
        {
            agent.stateMachine.ChangeState(AiStateID.Idle);
        }
        agent.navMeshAgent.destination = pickup.transform.position;
    }

    public void Exit(AIAgent agent)
    {
        if (isPickup)
        {
            pickup.GetComponent<HaveGunCheck>().isEnemyEquip = true;
            pickup.GetComponent<HaveGunCheck>().isEquip = true;

            if(agent.isReady)
            {
                animator.SetBool("Equip", pickup.GetComponent<HaveGunCheck>().isEnemyEquip);
                agent.rig.weight = 1f;
            }


            //나중에 rifle 정보 가지고 와서 바꾸기
            agent.rifleWeapons[2].SetActive(true);

            Destroy(pickup.gameObject);
        }
    }



    private void CheckPickup(AIAgent agent)
    {
        Collider[] a = Physics.OverlapSphere(agent.transform.position, 0.5f);
        foreach (Collider col in a)
        {
            if (col.CompareTag("Bullet") && !col.GetComponent<HaveGunCheck>().isEquip)
            {
                isPickup = true;
                agent.ammoRemain += agent.Nowgundata.magCapcity;
            }
        }
    }


    private GameObject FindClosestBullet(AIAgent agnet)
    {
        GameObject[] Bullets = GameObject.FindGameObjectsWithTag("Bullet");
        GameObject closestBullet = null;
        float closestDistance = float.MaxValue;

        foreach (var bullet in Bullets)
        {
            float distanceToWeapon = Vector3.Distance(agnet.transform.position, bullet.transform.position);
            if (distanceToWeapon < closestDistance)
            {
                if (!bullet.GetComponent<HaveGunCheck>().isEquip)
                {
                    closestDistance = distanceToWeapon;
                    closestBullet = bullet;
                }
            }
        }
        return closestBullet;
    }
}
