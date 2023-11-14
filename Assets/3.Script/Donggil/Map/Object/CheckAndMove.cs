using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAndMove : MonoBehaviour
{
    [Header("BoxCollider의 Center, Size 똑같이 입력")]
    public Vector3 colliderPos = Vector3.zero;
    public Vector3 size = Vector3.zero;

    [SerializeField] private bool isInMap = false;
    [SerializeField] private MapSize scale;

    [Header("탐지할 레이어 선택 (태그는 Wall)")]
    public Layers layers;

    [Header("오브젝트 비활성화 체크할때 서로 같은 레이어인가")]
    public bool isThislayerSame = false;

    private bool isPushEnd = false;

    public int targetFrame = 1000;

    private void Start()
    {
        scale = FindObjectOfType<MapSize>();
    }

    private void Update()
    {
        if (!isPushEnd)
        {
            IsExistCollider();
        }
    }

    private void IsExistCollider()
    {
        int layerMask = 1 << (int)layers;
        Collider[] colliders;
        Vector3 objectCenter = colliderPos + transform.position;

        if (isInMap)
        {
            colliders = Physics.OverlapBox(objectCenter, (size / 2) * scale.gameObject.transform.localScale.x, transform.rotation, layerMask);      //이 레이어만 탐지
        }
        else
        {
            colliders = Physics.OverlapBox(objectCenter, size / 2, transform.rotation, layerMask);      //이 레이어만 탐지
        }

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Wall"))
            {
                if (isThislayerSame)
                {
                    if (colliders.Length == 1) isPushEnd = true;
                    else if (colliders.Length > 1)
                    {
                        Vector3 closestPoint = col.ClosestPointOnBounds(objectCenter);
                        Vector3 dir = (closestPoint - objectCenter).normalized;
                        Vector3 whereToMove = new Vector3(dir.x, 0, dir.z);
                        transform.position -= whereToMove;
                        //Debug.Log(dir);
                        if (dir == Vector3.up || Time.frameCount > targetFrame)
                        {
                            isPushEnd = true;
                            //Debug.Log(isPushEnd);
                        }
                    }
                }
                else
                {
                    if (colliders.Length == 0) isPushEnd = true;
                    else if (colliders.Length > 0)
                    {
                        Vector3 closestPoint = col.ClosestPointOnBounds(objectCenter);
                        Vector3 dir = (closestPoint - objectCenter).normalized;
                        Vector3 whereToMove = new Vector3(dir.x, 0, dir.z);
                        transform.position -= whereToMove;
                        //Debug.Log(dir);
                        if(dir == Vector3.up || Time.frameCount > targetFrame)
                        {
                            isPushEnd = true;
                            //Debug.Log(isPushEnd);
                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (isInMap)
        {
            Gizmos.DrawCube(colliderPos + transform.position, size * scale.gameObject.transform.localScale.x);
        }
        else
        {
            Gizmos.DrawCube(colliderPos + transform.position, size);
        }

    }
}
