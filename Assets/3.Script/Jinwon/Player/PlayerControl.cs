using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Move")]
    public float walkSpeed;
    public float runSpeed;
    private float currentSpeed;
    public float moveSpeedX = 0; // �ִϸ����� ���� Ʈ���� ���� ��
    public float moveSpeedZ = 0; // �ִϸ����� ���� Ʈ���� ���� ��

    [Header("Battle")]
    private bool isEquip = false;
    public Transform leftHandMount;
    public Transform rightHandMount;

    // Components
    private Rigidbody rigidBody;
    private Animator animator;
    private PlayerInput playerInput;

    private void Awake()
    {
        TryGetComponent(out rigidBody);
        TryGetComponent(out animator);
        TryGetComponent(out playerInput);

        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        Move(); // �÷��̾� �̵�

        Debug.Log(transform.forward);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            EquipGun();
        }
    }

    private void Move()
    {
        if (playerInput.isRun && moveSpeedZ > 0) // �÷��̾� �ӵ� ����
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        rigidBody.MovePosition(rigidBody.position + currentSpeed * Time.deltaTime * transform.forward * moveSpeedZ); // Z�� �̵�
        rigidBody.MovePosition(rigidBody.position + currentSpeed * Time.deltaTime * transform.right * moveSpeedX); // X�� �̵�

        animator.SetFloat("MoveSpeedX", moveSpeedX); // x�� �Է°� ���� Ʈ���� ����
        animator.SetFloat("MoveSpeedZ", moveSpeedZ); // z�� �Է°� ���� Ʈ���� ����
    }

    private void EquipGun()
    {
        isEquip = true;
    }

    public void TPSZoomIn()
    {
        if (!isEquip)
        {
            return;
        }

        animator.SetBool("Aim", true);
        animator.SetTrigger("AimTrigger");
    }

    public void TPSZoomOut()
    {
        if (!isEquip)
        {
            return;
        }

        animator.SetBool("Aim", false);
    }

    /*private void OnAnimatorIK(int layerIndex)
    {
        if (!isEquip || !isAim)
        {
            return;
        }

        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }*/
}
