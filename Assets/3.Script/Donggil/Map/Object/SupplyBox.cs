using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyBox : MonoBehaviour
{
    public GameObject smoke;
    public GameObject parachute;

    public GameObject RareItemSpawn;

    private Rigidbody rigid;

    public bool isGrounded = true;

    private void Start()
    {
        TryGetComponent(out rigid);
        rigid.drag = 8;
    }


    private void Update()
    {
        if (isGrounded)
        {
            Grounded();
        }
    }

    private void Grounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        Debug.DrawRay(transform.position, Vector3.down * 3.0f, Color.red);
        if (Physics.Raycast(ray, 3.0f))
        {
            rigid.drag = 0;
            parachute.SetActive(false);
            smoke.SetActive(true);
            RareItemSpawn.SetActive(true);
            isGrounded = false;
        }
    }
}
