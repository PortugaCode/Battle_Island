using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove1 : MonoBehaviour
{
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;
    [SerializeField] private float Movespeed;
    [SerializeField] private Rigidbody rig;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private HelicopterHealth helicopterHealth;

    private void Start()
    {
        //GameObject.FindObjectOfType<EnemyHealth>().TryGetComponent(out enemyHealth);
        GameObject.FindObjectOfType<HelicopterHealth>().TryGetComponent(out helicopterHealth);
        TryGetComponent(out rig);
    }

    private void FixedUpdate()
    {
        rig.velocity = new Vector3(horizontal * Movespeed * Time.deltaTime, rig.velocity.y, vertical * Movespeed * Time.deltaTime);
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(KeyCode.Space))
        {
            helicopterHealth.TakeDamage(10f);
        }
    }
}
