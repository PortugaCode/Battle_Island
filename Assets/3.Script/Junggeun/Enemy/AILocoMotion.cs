using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILocoMotion : MonoBehaviour
{
    [SerializeField] private Transform playerTarget;
    private NavMeshAgent agent;
    private Animator animator;

    
    [SerializeField] private float maxTime = 1.0f;
    [SerializeField] private float maxDistance = 1.0f;
    private float timer = 0.0f;


    private void Start()
    {
        TryGetComponent(out agent);
        TryGetComponent(out animator);
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out playerTarget);
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0.0f)
        {
            float sqdistance = (playerTarget.position - agent.destination).sqrMagnitude;
            if(sqdistance > maxDistance* maxDistance)
            {
                agent.destination = playerTarget.position;
            }
            timer = maxTime;
        }



        
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }


}
