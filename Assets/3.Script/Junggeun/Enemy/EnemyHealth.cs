using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    private RagDoll ragdoll;
    private bool isDie = false;
    public bool IsDie => isDie;

    private SkinnedMeshRenderer[] skinnedMeshRenderer;

    [SerializeField] private float blinkIn;
    [SerializeField] private float blinkDu;
    private float blinkTimer;

    private UIHealthBar healthBar;


    private void Awake()
    {
        healthBar = GetComponentInChildren<UIHealthBar>();
        currentHealth = maxHealth;
        TryGetComponent(out ragdoll);
        skinnedMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    private void Start()
    {
        var rigbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rig in rigbodies)
        {
            HitBox hitbox = rig.gameObject.AddComponent<HitBox>();
            hitbox.enemyHealth = this;
        }
    }

    private void Update()
    {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDu);
        float intensity = (lerp * blinkIn) + 1.0f;
        for(int i = 0; i < skinnedMeshRenderer.Length; i++)
        {
            skinnedMeshRenderer[i].material.color = Color.white * intensity;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.SetHealthBar(currentHealth / maxHealth);
        if (currentHealth <= 0.0f)
        {
            Die();
        }
        blinkTimer = blinkDu;
    }

    private void Die()
    {
        isDie = true;
        ragdoll.ActivateRagDoll();
        healthBar.gameObject.SetActive(false);
    }
}
