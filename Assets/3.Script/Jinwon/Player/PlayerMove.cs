using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Player Move Stat")]
    public float walkSpeed;
    public float runSpeed;
    private float currentSpeed;
    public float moveSpeedX = 0; // 애니메이터 블렌드 트리에 사용될 값
    public float moveSpeedZ = 0; // 애니메이터 블렌드 트리에 사용될 값

    [Header("Player Move Direction")]
    public Vector3 direction;

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

        rigidBody.MovePosition(rigidBody.position + currentSpeed * Time.deltaTime * direction); // 입력 받은 방향대로 플레이어 이동
        animator.SetFloat("MoveSpeedX", moveSpeedX); // x축 입력값 블렌드 트리에 적용
        animator.SetFloat("MoveSpeedZ", moveSpeedZ); // z축 입력값 블렌드 트리에 적용
    }
}
