using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterMovement : MonoBehaviour
{
    // Components
    private Rigidbody rb;
    private Animator animator;
    private ZoomControl zoomControl;
    private CombatControl combatControl;

    // Movement
    [Header("Movement")]
    public bool canMove = true;
    private float x;
    private float z;
    public float currentSpeed;
    public float walkSpeed = 2.5f;
    public float runSpeed = 6.5f;
    public float forwardSpeed = 0f;
    public float firstPersonSpeed = 0.5f;
    public float thirdPersonSpeed = 1.0f;
    public bool isCrouch = false;
    private float crouchTimer = 0f;

    // Jump
    private float jumpForce = 5.0f;

    // Ground Check
    [Header("Ground Check")]
    public LayerMask playerLayer;
    public bool isGround = true;

    // Movement Bool
    private bool isRun = false;

    private void Awake()
    {
        TryGetComponent(out rb);
        TryGetComponent(out animator);
        TryGetComponent(out zoomControl);
        TryGetComponent(out combatControl);

        // [���콺 Ŀ�� ����]
        Cursor.lockState = CursorLockMode.Locked;

        // [�̵� �ӵ� �ʱ�ȭ]
        forwardSpeed = walkSpeed;
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        if (combatControl.isDead)
        {
            return;
        }

        if (canMove)
        {
            GetInput();
            GroundCheck();
        }

        if (isCrouch)
        {
            crouchTimer += Time.deltaTime;
        }

        if (!isRun && currentSpeed > walkSpeed) // �ӵ� õõ�� �پ���
        {
            currentSpeed -= Time.deltaTime * 10.0f;
            forwardSpeed = currentSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Insert)) // TEST
        {
            InventoryControl.instance.ShowInventory();
        }

        if (Input.GetKeyDown(KeyCode.Home)) // Test
        {
            animator.SetTrigger("Dance");
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
    }

    private void GetInput()
    {
        #region Ű���� �Է�
        // [���� �Է�]
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        // [�ִϸ����� ����]
        animator.SetFloat("MoveSpeedX", x);
        animator.SetFloat("MoveSpeedZ", z * currentSpeed);

        // [�ӵ� ����] - �ȱ�, �޸���
        if (Input.GetKey(KeyCode.LeftShift) && z > 0 && isGround)
        {
            if (isCrouch && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                isCrouch = false;
                crouchTimer = 0;
                animator.SetTrigger("UnCrouch");
            }

            isRun = true;

            if (forwardSpeed < runSpeed)
            {
                forwardSpeed += Time.deltaTime * 7.5f;
            }

            currentSpeed = forwardSpeed;
        }
        
        // [�ɱ�]
        if (isGround && combatControl.currentWeapon == Weapon.Gun && Input.GetKeyDown(KeyCode.C))
        {
            if (isCrouch)
            {
                isCrouch = false;
                crouchTimer = 0;
                animator.SetTrigger("UnCrouch");
                animator.SetBool("isCrouch", isCrouch);
            }
            else
            {
                isCrouch = true;
                animator.SetTrigger("Crouch");
                animator.SetBool("isCrouch", isCrouch);
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
        }

        // [����]
        if (isGround && Input.GetKeyDown(KeyCode.Space))
        {
            combatControl.timerOn = false;
            combatControl.clickTimer = 0;

            if (combatControl.isFirstPerson)
            {
                combatControl.isFirstPerson = false;
                currentSpeed = walkSpeed;
                zoomControl.First_ZoomOut();
            }
            else if (combatControl.isThirdPerson)
            {
                combatControl.isThirdPerson = false;
                currentSpeed = walkSpeed;
                zoomControl.Third_ZoomOut();
            }

            isCrouch = false;
            animator.SetBool("isCrouch", isCrouch);

            animator.SetTrigger("Jump");

            forwardSpeed = walkSpeed;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        #endregion

        // [�÷��̾� ȸ��]
        if (!combatControl.lookAround)
        {
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        }
    }

    private void Move()
    {
        // [�ӵ� ����] - Ⱦ�̵� �� �ӵ�
        float strafeSpeed;
        if (currentSpeed > walkSpeed)
        {
            strafeSpeed = walkSpeed;
        }
        else
        {
            strafeSpeed = currentSpeed;
        }

        // [�ӵ� ����] - ���� �� �ӵ�
        if (!isGround)
        {
            currentSpeed = walkSpeed;
        }

        // [�̵�]
        Vector3 targetDirection = transform.forward * z * currentSpeed + transform.right * x * strafeSpeed;

        if (isCrouch && crouchTimer > 0.95f && Vector3.Magnitude(targetDirection) > 0)
        {
            isCrouch = false;
            crouchTimer = 0;
            animator.SetTrigger("UnCrouch");
        }

        if (!isCrouch || (isCrouch && crouchTimer > 0.95f))
        {
            rb.velocity = new Vector3(targetDirection.x, rb.velocity.y, targetDirection.z);
        }
        
    }

    private void GroundCheck()
    {
        // [�÷��̾� ���̾�� ����]
        int layerMask = ~playerLayer.value;

        // [�÷��̾ ���� ����ִ��� üũ]
        isGround = Physics.OverlapSphere(transform.position, 0.2f, layerMask).Length > 0;
    }
}
