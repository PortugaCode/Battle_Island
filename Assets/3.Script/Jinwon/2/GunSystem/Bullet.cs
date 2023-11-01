using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
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

    private void OnCollisionEnter(Collision collision) // 충돌 처리
    {
        if (!collision.collider.CompareTag("Player")) // 플레이어 무시
        {
            Destroy(gameObject);
        }
    }

    private void DrawTrajectory()
    {

    }
}
