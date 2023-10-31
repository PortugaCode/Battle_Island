using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILocoMotion : MonoBehaviour
{
    [SerializeField] private Transform playerTarget;
    private NavMeshAgent agent;
    private Animator animator;

    
    [SerializeField] private float maxTime = 1.0f; // 멀어진 후 따라갈 때 얼마나 텀이 있을지
    [SerializeField] private float maxDistance = 1.0f; // 멀어지면 따라갈 때 최대 멀어지는 거리
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
            if(sqdistance > maxDistance* maxDistance) // 제곱을 하는 이유는 SqrMagitude는 제곱을 한 값을 반환하기 때문에
            {
                agent.destination = playerTarget.position;
            }
            timer = maxTime;
        }
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }


}
