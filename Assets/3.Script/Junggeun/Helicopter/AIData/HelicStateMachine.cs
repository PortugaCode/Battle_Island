using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicStateMachine
{
    public HelicState[] states;
    public HelicAgent agent;
    public HelicStateID currentState;

    public HelicStateMachine(HelicAgent agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(HelicStateID)).Length;
        states = new HelicState[numStates];
    }

    public void RegsisterState(HelicState state)
    {
        int index = (int)state.GetID();
        states[index] = state;
    }

    public HelicState GetState(HelicStateID stateID)
    {
        int index = (int)stateID;
        return states[index];
    }

    public void Update()
    {
        GetState(currentState)?.AIUpdate(agent); 
    }

    public void ChangeState(HelicStateID newstate)
    {
        GetState(currentState)?.Exit(agent);
        currentState = newstate;
        GetState(currentState)?.Enter(agent);
    }
}
