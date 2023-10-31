using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    private AIStateMachine stateMachine;
    public AiStateID initalState;

    private void Awake()
    {
        stateMachine = new AIStateMachine(this);
        stateMachine.ChangeState(initalState);
    }

    private void Update()
    {
        stateMachine.Update();
    }
}
