using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiStateID
{
    ChasePlayer,
    Death
}

public interface AIState
{
    AiStateID GetID();
    void Enter(AIAgent agent);
    void Update(AIAgent agent);
    void Exit(AIAgent agent);
}
