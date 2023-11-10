using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAndMove : MonoBehaviour
{
    [Header("BoxCollider의 Center, Size 똑같이 입력")]
    public Vector3 colliderPos = Vector3.zero;
    public Vector3 size = Vector3.zero;

    private void Update()
    {
        IsExistCollider();
    }

    private void IsExistCollider()
    {
        Collider[] colliders;
        Vector3 objectCenter = colliderPos + transform.position;
        colliders = Physics.OverlapBox(objectCenter, size / 2, transform.rotation, 1 << LayerMask.NameToLayer("Ground")) ;

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Wall"))
            {
                if (colliders.Length == 1) return;
                else if (colliders.Length > 1)
                {
                    Vector3 closestPoint = col.ClosestPoint(objectCenter);
                    Vector3 dir = (closestPoint - objectCenter).normalized;
                    Vector3 whereToMove = new Vector3(dir.x, 0, dir.z);
                    transform.position -= whereToMove;
                    Debug.Log(dir);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawCube(colliderPos + transform.position, size);

    }
}
