using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterHealth : MonoBehaviour
{
    [SerializeField] private float Maxhp;
    [SerializeField] private float Curhp;
    [SerializeField] private GameObject helicopter;
    public float CurHp => Curhp;

    private HelicAgent agent;
    [SerializeField] private GameObject DieEffect;
    [SerializeField] private GameObject DieEffect2;
    [SerializeField] private GameObject Helic;

    private void Awake()
    {
        Curhp = Maxhp;
        TryGetComponent(out agent);
        helicopter = transform.GetChild(1).GetChild(0).transform.gameObject;
    }

    private void Update()
    {
        if (GameManager.instance.isWin) Die();
    }


    public void TakeDamage(float damege)
    {
        Curhp -= damege;

        if(Curhp <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        agent.stateMachine.ChangeState(HelicStateID.Die);
        GameObject a = Instantiate(DieEffect, helicopter.transform.position, Quaternion.identity);
        GameObject b = Instantiate(DieEffect2, helicopter.transform.position, Quaternion.identity);
        GameObject c = Instantiate(Helic, helicopter.transform.position, helicopter.transform.rotation);
        c.GetComponent<Rigidbody>().isKinematic = false;
        c.GetComponent<Rigidbody>().useGravity = true;
        Destroy(a, 6f);
        Destroy(b, 6f);
        Destroy(c, 3f);
        Destroy(gameObject);
    }
}
