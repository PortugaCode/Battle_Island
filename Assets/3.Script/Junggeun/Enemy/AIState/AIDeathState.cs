using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    public Vector3 direction;
    public AiStateID GetID()
    {
        return AiStateID.Death;
    }

    public void Enter(AIAgent agent)
    {
        agent.navMeshAgent.velocity = Vector3.zero;
        direction.y = 1f;
        agent.ragDoll.ActivateRagDoll();
        agent.ragDoll.ApplyForce(direction * agent.config.dieForce);
        agent.ui.gameObject.SetActive(false);

        //나중에 건 데이터에 맞게 밖으로 보내기 (죽을 때 총 밖으로 꺼내는 작업)
        agent.rifleWeapons[2].GetComponent<Rigidbody>().useGravity = true;
        agent.rifleWeapons[2].GetComponent<Rigidbody>().isKinematic = false;
        agent.rifleWeapons[2].transform.SetParent(null);
    }

    public void AIUpdate(AIAgent agent)
    {
        agent.navMeshAgent.velocity = Vector3.zero;
    }

    public void Exit(AIAgent agent)
    {
        
    }
}
