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
        if (pickup == null)
        {
            agent.stateMachine.ChangeState(AiStateID.RandomMove);
            return;
        }
        agent.navMeshAgent.destination = pickup.transform.position;
        agent.navMeshAgent.speed = 5;
    }

    public void AIUpdate(AIAgent agent)
    {
        if (pickup == null)
        {
            agent.stateMachine.ChangeState(AiStateID.RandomMove);
            return;
        }

        else if (pickup.GetComponent<EquipCheck>().isEquip)
        {
            pickup = FindClosestWeapon(agent);
            if(pickup != null)
            {
                agent.stateMachine.ChangeState(AiStateID.RandomMove);
                return;
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

            //나중에 rifle 정보 가지고 와서 바꾸기
            agent.enemyAudio.PlayReload();

            //MonoBehaviour.Destroy(pickup.gameObject);
        }
    }



    private void CheckPickup(AIAgent agent)
    {
        Collider[] a = Physics.OverlapSphere(agent.transform.position, 0.5f, agent.ItemLayer);
        foreach(Collider col in a)
        {
            if(col.CompareTag("Weapon") && !col.GetComponent<EquipCheck>().isEquip)
            {
                isPickup = true;
                agent.isReady = true;
                agent.isAmmoReady = true;
                agent.CurrentGun = col.gameObject;
                agent.CurrentGun.transform.SetParent(agent.GunPivot.transform);
                agent.CurrentGun.transform.localPosition = Vector3.zero;
                agent.CurrentGun.transform.localRotation = Quaternion.Euler(Vector3.zero);
                agent.CurrentGun.GetComponent<Rigidbody>().isKinematic = true;
                agent.CurrentGun.GetComponent<Rigidbody>().useGravity = false;
                agent.CurrentGun_Gun = agent.CurrentGun.GetComponent<Gun>();
                agent.Nowgundata = agent.gundata[(int)col.GetComponent<Gun>().gunType];
                if(agent.CurrentGun_Gun.gunType == GunType.Sniper1)
                {
                    agent.enemyAudio.ChangeSniperSound();
                }
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
