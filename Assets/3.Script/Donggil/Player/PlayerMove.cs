using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rigid;
    public float speed = 10f;

    private void Awake()
    {
        TryGetComponent(out rigid);
    }
    private void Update()
    {
        float move_x = Input.GetAxis("Horizontal");
        float move_z = Input.GetAxis("Vertical");

        Vector3 Direction = new Vector3(move_x, 0, move_z) * speed;

        rigid.velocity = Direction;
    }

}
