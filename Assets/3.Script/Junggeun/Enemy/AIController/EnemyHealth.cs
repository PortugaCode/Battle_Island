using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    private AIAgent agent;
    private bool isDie = false;
    public bool IsDie => isDie;
    public bool isDamage = false;
    public bool isDeadZone = false;

    [SerializeField] private SkinnedMeshRenderer[] skinnedMeshRenderer;

    [SerializeField] private float blinkIn;
    [SerializeField] private float blinkDu;
    private float blinkTimer;

    private UIHealthBar healthBar;
    private AILocoMotion aILocoMotion;
    [SerializeField] private DeadZone deadZone;

    public DeadZone.Phase phase;
    private float gameTime = 0;

    private void Start()
    {
        healthBar = GetComponentInChildren<UIHealthBar>();
        deadZone = FindObjectOfType<DeadZone>();
        currentHealth = maxHealth;
        TryGetComponent(out agent);
        TryGetComponent(out aILocoMotion);
        skinnedMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        var rigbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rig in rigbodies)
        {
            HitBox hitbox = rig.gameObject.AddComponent<HitBox>();
            hitbox.enemyHealth = this;
        }
    }

    private void FixedUpdate()
    {
        isDamage = false;
    }

    private void Update()
    {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDu);
        float intensity = (lerp * blinkIn) + 1.0f;
        for (int i = 0; i < skinnedMeshRenderer.Length; i++)
        {
            skinnedMeshRenderer[i].material.color = Color.white * intensity;
        }

        gameTime += Time.deltaTime;
        if (isDeadZone)
        {
            if (gameTime >= 1.0f)
            {
                TakeDamageDeadZone(deadZone.SetDeadZoneDamage(phase), Vector3.zero);
                gameTime = 0;
            }
        }
    }

    public void TakeDamage(float amount, Vector3 direction)
    {

        isDamage = true;

        currentHealth -= amount;
        healthBar.SetHealthBar(currentHealth / maxHealth);
        if (currentHealth <= 0.0f && !isDie && !GameManager.instance.isLastEnemy)
        {
            Die(direction);
        }
        else if (currentHealth <= 0.0f && !isDie && GameManager.instance.isLastEnemy)
        {
            aILocoMotion.isAlreadyDie = true;
            aILocoMotion.isAlreadyDie2 = true;
            GameManager.instance.isLastEnemy = false;
        }
        else if(currentHealth <= 0.0f && isDie) DieEffect(direction);
        blinkTimer = blinkDu;
    }

    public void TakeDamageDeadZone(float amount, Vector3 direction)
    {
        currentHealth -= amount;
        healthBar.SetHealthBar(currentHealth / maxHealth);
        if (currentHealth <= 0.0f && !isDie && !aILocoMotion.isAlreadyDie2)
        {
            Die(direction);
        }
        if(!aILocoMotion.isAlreadyDie2) blinkTimer = blinkDu;
    }

    private void Die(Vector3 direction)
    {
        isDie = true;
        GameManager.instance.enemyCount -= 1;
        GameManager.instance.killCount += 1;
        if (GameManager.instance.enemyCount <= 0)
        {
            GameManager.instance.isWin = true;
            GameManager.instance.isGameOver = true;
        }
        else if (GameManager.instance.enemyCount == 1)
        {
            GameManager.instance.isLastEnemy = true;
        }
        AIDeathState deathState = agent.stateMachine.GetState(AiStateID.Death) as AIDeathState;
        deathState.direction = direction;
        agent.stateMachine.ChangeState(AiStateID.Death);
        agent.navMeshAgent.velocity = Vector3.zero;
    }

    private void DieEffect(Vector3 direction)
    {
        AIDeathState deathState = agent.stateMachine.GetState(AiStateID.Death) as AIDeathState;
        deathState.direction = direction;
        agent.stateMachine.ChangeState(AiStateID.Death);
        agent.navMeshAgent.velocity = Vector3.zero;
    }
}
