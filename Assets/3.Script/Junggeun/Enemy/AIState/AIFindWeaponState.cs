using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AIFindWeaponState : AIState
{
    private GameObject pickup;
    private bool isPickup = false;
    private Animator animator;
    

    public AiStateID GetID()
    {
        return AiStateID.FindWeapon;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("총 찾기");

        animator = agent.gameObject.GetComponent<Animator>();
        pickup = FindClosestWeapon(agent);
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
            pickup = FindClosestWeapon(agent);
            if(pickup != null)
            {
                agent.stateMachine.ChangeState(AiStateID.RandomMove);
            }
        }


        CheckPickup(agent);

        if (isPickup)
        {
            agent.stateMachine.ChangeState(AiStateID.Idle);
        }
    }

    public void Exit(AIAgent agent)
    {
        if(isPickup)
        {
            pickup.GetComponent<HaveGunCheck>().isEnemyEquip = true;
            pickup.GetComponent<HaveGunCheck>().isEquip = true;
            animator.SetBool("Equip", pickup.GetComponent<HaveGunCheck>().isEnemyEquip);
            agent.rig.weight = 1f;
            
            //나중에 rifle 정보 가지고 와서 바꾸기
            agent.rifleWeapons[2].SetActive(true);

            MonoBehaviour.Destroy(pickup.gameObject);
        }
    }



    private void CheckPickup(AIAgent agent)
    {
        Collider[] a = Physics.OverlapSphere(agent.transform.position, 0.5f);
        foreach(Collider col in a)
        {
            if(col.CompareTag("Weapon") && !col.GetComponent<HaveGunCheck>().isEquip)
            {
                isPickup = true;
                agent.isReady = true;
                agent.isAmmoReady = true;
                agent.Nowgundata = agent.gundata[(int)col.GetComponent<GunEnum>().gunState];
                agent.magAmmo = agent.Nowgundata.magCapcity;
                agent.ammoRemain += agent.Nowgundata.magCapcity;
            }
        }
    }


    private GameObject FindClosestWeapon(AIAgent agnet)
    {
        GameObject[] Weapons = GameObject.FindGameObjectsWithTag("Weapon");
        GameObject closestWeapon = null;
        float closestDistance = float.MaxValue;

        foreach(var weapon in Weapons)
        {
            float distanceToWeapon = Vector3.Distance(agnet.transform.position, weapon.transform.position);
            if(distanceToWeapon < closestDistance)
            {
                if(!weapon.GetComponent<HaveGunCheck>().isEquip)
                {
                    closestDistance = distanceToWeapon;
                    closestWeapon = weapon;
                }
            }
        }
        return closestWeapon;
    }

}
