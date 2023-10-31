using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDoll : MonoBehaviour
{
    //RagDoll = 캐릭터의 머리, 팔, 발 등 콜라이더를 따로 생성하는 작업
    [SerializeField] private Rigidbody[] rigidbodies;
    private Animator animator; 

    private void Awake()
    {
        TryGetComponent(out animator);
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        DeActivateRagDoll();
    }

    public void DeActivateRagDoll()
    {
        foreach(var rig in rigidbodies)
        {
            rig.isKinematic = true;
        }
        animator.enabled = true;
    }

    public void ActivateRagDoll()
    {
        foreach (var rig in rigidbodies)
        {
            rig.isKinematic = false;
        }
        animator.enabled = false;
    }
}
