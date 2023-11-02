using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 1.0f;
    public float bulletDamage = 0;

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
            if (collision.collider.GetComponent<Damagable>()) // 데미지 받을 수 있으면 호출 (Damageble은 현재 테스트로 작성해놓은 스크립트, 합칠때 수정)
            {
                collision.collider.GetComponent<Damagable>().TakeDamage(bulletDamage);
                Debug.Log($"{collision.collider.name}에게 {bulletDamage}의 데미지 입힘");
            }

            Destroy(gameObject);
        }
    }
}
