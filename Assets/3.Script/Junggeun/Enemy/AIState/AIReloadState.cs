using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIReloadState : AIState
{
    private Animator animator;

    public AiStateID GetID()
    {
        return AiStateID.Reload;
    }

    public void Enter(AIAgent agent)
    {
        Debug.Log("¿Á¿Â¿¸");
        animator = agent.gameObject.GetComponent<Animator>();

        if (agent.ammoRemain <= 0)
        {
            agent.isAmmoReady = false;
            animator.SetBool("Equip", agent.isAmmoReady);
            agent.twoBoneIK.weight = 0f;
            agent.rig.weight = 0f;
            agent.stateMachine.ChangeState(AiStateID.RandomMove);
            return;
        }
        else
        {
            animator.SetTrigger("Reload");
            agent.isAmmoReady = true;
            agent.isneedReload = true;
        }


        
    }

    public void AIUpdate(AIAgent agent)
    {
        if(agent.isneedReload == false)
        {
            agent.stateMachine.ChangeState(AiStateID.RandomMove);
        }
    }

    public void Exit(AIAgent agent)
    {
    }


}
