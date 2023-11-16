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

    [SerializeField] private SkinnedMeshRenderer[] skinnedMeshRenderer;

    [SerializeField] private float blinkIn;
    [SerializeField] private float blinkDu;
    private float blinkTimer;

    private UIHealthBar healthBar;


    private void Start()
    {
        healthBar = GetComponentInChildren<UIHealthBar>();
        currentHealth = maxHealth;
        TryGetComponent(out agent);
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
    }

    public void TakeDamage(float amount, Vector3 direction)
    {

        isDamage = true;

        currentHealth -= amount;
        healthBar.SetHealthBar(currentHealth / maxHealth);
        if (currentHealth <= 0.0f)
        {
            Die(direction);
        }
        blinkTimer = blinkDu;

    }

    public void TakeDamageDeadZone(float amount, Vector3 direction)
    {
        currentHealth -= amount;
        healthBar.SetHealthBar(currentHealth / maxHealth);
        if (currentHealth <= 0.0f)
        {
            Die(direction);
        }
        blinkTimer = blinkDu;
    }

    private void Die(Vector3 direction)
    {
        isDie = true;
        
        AIDeathState deathState = agent.stateMachine.GetState(AiStateID.Death) as AIDeathState;
        deathState.direction = direction;
        agent.stateMachine.ChangeState(AiStateID.Death);
        agent.navMeshAgent.velocity = Vector3.zero;
    }
}
