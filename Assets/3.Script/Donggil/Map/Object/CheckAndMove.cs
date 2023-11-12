using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAndMove : MonoBehaviour
{
    [Header("BoxCollider�� Center, Size �Ȱ��� �Է�")]
    public Vector3 colliderPos = Vector3.zero;
    public Vector3 size = Vector3.zero;


    [Header("Ž���� ���̾� ����")]
    public Layers layers;

    private void Update()
    {
        IsExistCollider();
    }

    private void IsExistCollider()
    {
        int layerMask = 1 << (int)layers;
        Collider[] colliders;
        Vector3 objectCenter = colliderPos + transform.position;
        colliders = Physics.OverlapBox(objectCenter, size / 2, transform.rotation, layerMask) ;      //�� ���̾ Ž��

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Wall"))
            {
                if (colliders.Length == 0) return;
                else if (colliders.Length > 0)
                {
                    Vector3 closestPoint = col.ClosestPointOnBounds(objectCenter);
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
