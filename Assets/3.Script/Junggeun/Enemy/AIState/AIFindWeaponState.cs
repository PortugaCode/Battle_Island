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
        Debug.Log("�� ã��");

        animator = agent.gameObject.GetComponent<Animator>();
        pickup = FindClosestWeapon(agent);
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
            pickup.GetComponent<EquipCheck>().isEnemyEquip = true;
            pickup.GetComponent<EquipCheck>().isEquip = true;
            animator.SetBool("Equip", pickup.GetComponent<EquipCheck>().isEnemyEquip);
            agent.rig.weight = 1f;

            //���߿� rifle ���� ������ �ͼ� �ٲٱ�
            agent.SelectRifleWeapons.SetActive(true);

            MonoBehaviour.Destroy(pickup.gameObject);
        }
    }



    private void CheckPickup(AIAgent agent)
    {
        Collider[] a = Physics.OverlapSphere(agent.transform.position, 0.5f);
        foreach(Collider col in a)
        {
            if(col.CompareTag("Weapon") && !col.GetComponent<EquipCheck>().isEquip)
            {
                isPickup = true;
                agent.isReady = true;
                agent.isAmmoReady = true;
                agent.SelectStartAim = agent.StartAim[(int)col.GetComponent<GunEnum>().gunState];
                agent.SelectRifleWeapons = agent.rifleWeapons[(int)col.GetComponent<GunEnum>().gunState];
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
                if(!weapon.GetComponent<EquipCheck>().isEquip)
                {
                    closestDistance = distanceToWeapon;
                    closestWeapon = weapon;
                }
            }
        }
        return closestWeapon;
    }

}
