using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILocoMotion : MonoBehaviour
{

    private NavMeshAgent agent;
    private Animator animator;

    private void Start()
    {
        TryGetComponent(out agent);
        TryGetComponent(out animator);
    }

    private void Update()
    {
        if (agent.hasPath)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }


}
