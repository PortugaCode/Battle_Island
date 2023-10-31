using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    private RagDoll ragdoll;


    private void Awake()
    {
        currentHealth = maxHealth;
        TryGetComponent(out ragdoll);

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

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if(currentHealth <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        ragdoll.ActivateRagDoll();
    }
}
