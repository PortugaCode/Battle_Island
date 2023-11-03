using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DebugNavMashAgent : MonoBehaviour
{
    public bool isvelocity;
    public bool desiredvelocity;
    public bool ispath;
    NavMeshAgent agent;

    private void Awake()
    {
        TryGetComponent(out agent);
    }

    private void OnDrawGizmos()
    {
        // 방향성
        if(isvelocity)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + agent.velocity);
        }

        // 목표하는 방향
        if (desiredvelocity)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + agent.desiredVelocity);
        }

        //목적지까지의 선 + 코너마다 확인
        if(ispath)
        {
            Gizmos.color = Color.black;
            var agentpath = agent.path;
            Vector3 prevCorner = transform.position;

            foreach(var corner in agentpath.corners)
            {
                Gizmos.DrawLine(prevCorner, corner);
                Gizmos.DrawSphere(corner, 0.1f);
                prevCorner = corner;
            }
        }

    }
}
