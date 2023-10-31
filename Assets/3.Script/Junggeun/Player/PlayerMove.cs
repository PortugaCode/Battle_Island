using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;
    [SerializeField] private float Movespeed;
    [SerializeField] private Rigidbody rig;

    private void Awake()
    {
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
    }
}
