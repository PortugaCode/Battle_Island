using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicDieState : HelicState
{
    public HelicStateID GetID()
    {
        return HelicStateID.Die;
    }

    public void Enter(HelicAgent agent)
    {
        agent.GetComponentInChildren<Rigidbody>().useGravity = true;
        agent.GetComponentInChildren<Rigidbody>().isKinematic = false;
    }


    public void AIUpdate(HelicAgent agent)
    {

    }

    public void Exit(HelicAgent agent)
    {

    }


}
