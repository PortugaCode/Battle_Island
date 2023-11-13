using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HelicStateID
{
    RandomMove,
    ChasePlayer,
    Shooting,
    BackMove,
    Die
}

public interface HelicState
{
    HelicStateID GetID();
    void Enter(HelicAgent agent);
    void AIUpdate(HelicAgent agent);
    void Exit(HelicAgent agent);

}
