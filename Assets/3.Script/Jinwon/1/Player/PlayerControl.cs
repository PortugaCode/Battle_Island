using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Move")]
    public float walkSpeed;
    public float runSpeed;
    private float currentSpeed;
    public float moveSpeedX = 0; // 애니메이터 블렌드 트리에 사용될 값
    public float moveSpeedZ = 0; // 애니메이터 블렌드 트리에 사용될 값

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
        Move(); // 플레이어 이동

        Debug.Log(transform.forward);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            EquipGun();
        }
    }

    private void Move()
    {
        if (playerInput.isRun && moveSpeedZ > 0) // 플레이어 속도 조정
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        rigidBody.MovePosition(rigidBody.position + currentSpeed * Time.deltaTime * transform.forward * moveSpeedZ); // Z축 이동
        rigidBody.MovePosition(rigidBody.position + currentSpeed * Time.deltaTime * transform.right * moveSpeedX); // X축 이동

        animator.SetFloat("MoveSpeedX", moveSpeedX); // x축 입력값 블렌드 트리에 적용
        animator.SetFloat("MoveSpeedZ", moveSpeedZ); // z축 입력값 블렌드 트리에 적용
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
