using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBullet : MonoBehaviour
{
    public float gravity = 9.8f;
    public float bulletSpeed = 1.0f;

    private Rigidbody rb;

    private void Awake()
    {
        TryGetComponent(out rb);
    }

    private void Start()
    {
        rb.velocity = transform.forward * bulletSpeed; // Bullet 앞으로 이동
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision) // 충돌 처리
    {
        if (!collision.collider.CompareTag("Player")) // 무시
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                Debug.Log("GAME OVER");
            }

            Destroy(gameObject);
        }
    }
}
