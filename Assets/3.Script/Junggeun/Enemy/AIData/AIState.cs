using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiStateID
{
    ChasePlayer,
    Death,
    Idle,
    FindWeapon
}

public interface AIState
{
    AiStateID GetID();
    void Enter(AIAgent agent);
    void AIUpdate(AIAgent agent);
    void Exit(AIAgent agent);
}
