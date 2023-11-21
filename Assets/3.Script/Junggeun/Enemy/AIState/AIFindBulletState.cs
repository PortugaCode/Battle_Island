using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFindBulletState : AIState
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
        Debug.Log("ÃÑ¾Ë Ã£±â");

        animator = agent.gameObject.GetComponent<Animator>();
        pickup = FindClosestBullet(agent);
        agent.navMeshAgent.destination = pickup.transform.position;
        agent.navMeshAgent.speed = 5;
    }

    public void AIUpdate(AIAgent agent)
    {
        if (pickup == null)
        {
            agent.stateMachine.ChangeState(AiStateID.RandomMove);
        }

        else if (pickup.GetComponent<EquipCheck>().isEquip)
        {
            pickup = FindClosestBullet(agent);
            if (pickup != null)
            {
                agent.stateMachine.ChangeState(AiStateID.RandomMove);
                return;
            }
            //agent.navMeshAgent.destination = pickup.transform.position;
        }


        CheckPickup(agent);

        if (isPickup)
        {
            agent.ammoRemain += 30;
            agent.stateMachine.ChangeState(AiStateID.Idle);
            return;
        }
    }

    public void Exit(AIAgent agent)
    {
        if (isPickup)
        {
            pickup.GetComponent<EquipCheck>().isEnemyEquip = true;
            pickup.GetComponent<EquipCheck>().isEquip = true;

            if(agent.isReady)
            {
                animator.SetBool("Equip", pickup.GetComponent<EquipCheck>().isEnemyEquip);
                agent.rig.weight = 1f;
            }

            MonoBehaviour.Destroy(pickup.gameObject);
        }
    }



    private void CheckPickup(AIAgent agent)
    {
        Collider[] a = Physics.OverlapSphere(agent.transform.position, 0.5f, agent.ItemLayer);
        foreach (Collider col in a)
        {
            if (col.CompareTag("AmmoBox") && !col.GetComponent<EquipCheck>().isEquip)
            {
                isPickup = true;
            }
        }
    }


    private GameObject FindClosestBullet(AIAgent agnet)
    {
        GameObject[] Bullets = GameObject.FindGameObjectsWithTag("AmmoBox");
        GameObject closestBullet = null;
        float closestDistance = float.MaxValue;

        foreach (var bullet in Bullets)
        {
            float distanceToWeapon = Vector3.Distance(agnet.transform.position, bullet.transform.position);
            if (distanceToWeapon < closestDistance)
            {
                if (!bullet.GetComponent<EquipCheck>().isEquip)
                {
                    closestDistance = distanceToWeapon;
                    closestBullet = bullet;
                }
            }
        }
        return closestBullet;
    }
}
