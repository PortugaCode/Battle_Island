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
        if(isvelocity)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + agent.velocity);
        }
        if (desiredvelocity)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + agent.desiredVelocity);
        }
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
