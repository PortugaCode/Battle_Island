using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFindArmorState : AIState
{
    private GameObject pickup;
    private bool isPickup = false;


    public AiStateID GetID()
    {
        return AiStateID.FindArmor;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("방어구 찾기");
        pickup = FindClosestArmor(agent);
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
            pickup = FindClosestArmor(agent);
            if (pickup != null)
            {
                agent.stateMachine.ChangeState(AiStateID.RandomMove);
            }
        }


        CheckPickup(agent);

        if (isPickup)
        {
            if(agent.SelectArmor == agent.Armor[0])
            {
                agent.GetComponent<EnemyHealth>().currentHealth += 20f;
                agent.GetComponent<EnemyHealth>().maxHealth += 20f;
            }
            else if (agent.SelectArmor == agent.Armor[1])
            {
                agent.GetComponent<EnemyHealth>().currentHealth += 40f;
                agent.GetComponent<EnemyHealth>().maxHealth += 40f;
            }
            else if (agent.SelectArmor == agent.Armor[2])
            {
                agent.GetComponent<EnemyHealth>().currentHealth += 60f;
                agent.GetComponent<EnemyHealth>().maxHealth += 60f;
            }

            agent.stateMachine.ChangeState(AiStateID.RandomMove);
        }
    }

    public void Exit(AIAgent agent)
    {
        if (isPickup)
        {
            pickup.GetComponent<EquipCheck>().isEnemyEquip = true;
            pickup.GetComponent<EquipCheck>().isEquip = true;
            agent.rig.weight = 1f;

            //나중에 rifle 정보 가지고 와서 바꾸기
            agent.SelectArmor.SetActive(true);

            MonoBehaviour.Destroy(pickup.gameObject);
        }
    }



    private void CheckPickup(AIAgent agent)
    {
        Collider[] a = Physics.OverlapSphere(agent.transform.position, 0.5f);
        foreach (Collider col in a)
        {
            if (col.CompareTag("Armor") && !col.GetComponent<EquipCheck>().isEquip)
            {
                isPickup = true;
                agent.isArmor = true;
                agent.SelectArmor = agent.Armor[(int)col.GetComponent<GunEnum>().armorState];
            }
        }
    }


    private GameObject FindClosestArmor(AIAgent agnet)
    {
        GameObject[] Armors = GameObject.FindGameObjectsWithTag("Armor");
        GameObject closestArmor = null;
        float closestDistance = float.MaxValue;

        foreach (var armor in Armors)
        {
            float distanceToWeapon = Vector3.Distance(agnet.transform.position, armor.transform.position);
            if (distanceToWeapon < closestDistance)
            {
                if (!armor.GetComponent<EquipCheck>().isEquip)
                {
                    closestDistance = distanceToWeapon;
                    closestArmor = armor;
                }
            }
        }
        return closestArmor;
    }
}
