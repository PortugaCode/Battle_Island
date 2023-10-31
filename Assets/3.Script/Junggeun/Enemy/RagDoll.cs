using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDoll : MonoBehaviour
{
    //RagDoll = ĳ������ �Ӹ�, ��, �� �� �ݶ��̴��� ���� �����ϴ� �۾�
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

    public void ApplyForce(Vector3 force)
    {
        var rigidBody = animator.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();
        rigidBody.AddForce(force, ForceMode.VelocityChange);
    }
}
