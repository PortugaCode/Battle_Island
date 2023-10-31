using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Player Move Stat")]
    public float walkSpeed;
    public float runSpeed;
    private float currentSpeed;
    public float moveSpeedX = 0; // �ִϸ����� ���� Ʈ���� ���� ��
    public float moveSpeedZ = 0; // �ִϸ����� ���� Ʈ���� ���� ��

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
        Move(); // �÷��̾� �̵�
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

        rigidBody.MovePosition(rigidBody.position + currentSpeed * Time.deltaTime * direction); // �Է� ���� ������ �÷��̾� �̵�
        animator.SetFloat("MoveSpeedX", moveSpeedX); // x�� �Է°� ���� Ʈ���� ����
        animator.SetFloat("MoveSpeedZ", moveSpeedZ); // z�� �Է°� ���� Ʈ���� ����
    }
}
