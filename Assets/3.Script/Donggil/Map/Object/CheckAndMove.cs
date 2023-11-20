using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAndMove : MonoBehaviour
{
    public enum selectTag
    {
        Wall = 0,
        Dongil01 = 1,
        Dongil02 = 2
    }

    [Header("BoxCollider의 Center, Size 똑같이 입력")]
    public Vector3 colliderPos = Vector3.zero;
    public Vector3 size = Vector3.zero;

    [SerializeField] private bool isInMap = false;
    [SerializeField] private MapSize scale;

    [Header("탐지할 레이어 선택")]
    public Layers layers;

    [Header("탐지할 태그 선택")]
    public selectTag tagName;

    [Header("검사하는 오브젝트가 서로 같은 레이어인가")]
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
        string tagString = Enum.GetName(typeof(selectTag), tagName);
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
            if (col.CompareTag(tagString))
            {
                if (isThislayerSame)
                {
                    if (colliders.Length == 1)
                    {
                        isPushEnd = true;
                        SetStatic(transform);
                    }
                    else if (colliders.Length > 1)
                    {
                        Vector3 closestPoint = col.ClosestPointOnBounds(objectCenter);
                        Vector3 dir = (closestPoint - objectCenter).normalized;
                        Vector3 whereToMove = new Vector3(dir.x, 0, dir.z);
                        transform.position -= whereToMove;
                        //Debug.Log(dir);
                        if (Time.frameCount > targetFrame)
                        {
                            isPushEnd = true;
                        }
                    }
                }
                else
                {
                    if (colliders.Length == 0)
                    {
                        isPushEnd = true;
                        SetStatic(transform);
                    }
                    else if (colliders.Length > 0)
                    {
                        Vector3 closestPoint = col.ClosestPointOnBounds(objectCenter);
                        Vector3 dir = (closestPoint - objectCenter).normalized;
                        Vector3 whereToMove = new Vector3(dir.x, 0, dir.z);
                        transform.position -= whereToMove;
                        //Debug.Log(dir);
                        if (Time.frameCount > targetFrame)
                        {
                            isPushEnd = true;
                        }
                    }
                }
            }
        }
    }
    private void SetStatic(Transform parent)
    {
        parent.gameObject.isStatic = true;

        foreach (Transform child in parent)
        {
            SetStatic(child);
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
