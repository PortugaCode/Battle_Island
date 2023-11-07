using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private GameObject bulletHolePrefab;

    public float gravity = 9.8f;
    public float bulletSpeed = 1.0f;
    public float bulletDamage = 0;

    public RaycastHit hit;

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
            if (collision.collider.GetComponent<Damagable>()) // 데미지 받을 수 있으면 호출 (Damageble은 현재 테스트로 작성해놓은 스크립트, 합칠때 수정)
            {
                collision.collider.GetComponent<Damagable>().TakeDamage(bulletDamage);
                //Debug.Log($"{collision.collider.name}에게 {bulletDamage}의 데미지 입힘");
            }

            Vector3 hitDirection = (collision.contacts[0].point - transform.position).normalized;


            if (!collision.collider.CompareTag("Wall")) // 벽, 바닥에만 나오게 변경 필요
            {
                GameObject hitEffect = Instantiate(hitEffectPrefab, collision.contacts[0].point, Quaternion.Euler(hitDirection));
                Destroy(hitEffect, 0.5f);

                if (hit.point != null)
                {
                    GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                    Destroy(bulletHole, 10.0f);
                }
            }

            if (collision.collider.CompareTag("Enemy"))
            {
                collision.collider.GetComponent<EnemyHealth>().TakeDamage(bulletDamage, collision.contacts[0].point);
            }

            Destroy(gameObject);
        }
    }
}
