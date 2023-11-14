using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILocoMotion : MonoBehaviour
{
    private NavMeshAgent agent;
    private AIAgent agent2;
    private Animator animator;
    private float originspeed;
    public bool isAlreadyDie = false;
    public bool isAlreadyDie2 = false;

    private void Start()
    {
        TryGetComponent(out agent);
        TryGetComponent(out agent2);
        TryGetComponent(out animator);
        originspeed = agent.speed;
    }

    private void Update()
    {

        if (agent.hasPath && !isAlreadyDie)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
        else if(!isAlreadyDie)
        {
            animator.SetFloat("Speed", 0);
        }

        if(isAlreadyDie && isAlreadyDie2)
        {
            isAlreadyDie2 = false;
            ToggleAnimationPause(true);
            agent2.stateMachine.ChangeState(AiStateID.AlreadyDie);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleAnimationPause(true);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            ToggleAnimationPause(false);
        }
    }


    public void ToggleAnimationPause(bool a)
    {
        if(a)
        {
            // 특정 프레임에서 애니메이션 멈춤
            animator.speed = 0f;
            agent.speed = 0f;
        }
        else if(!a)
        {
            // 애니메이션 시작
            animator.speed = 1f;
        }
    }

}
