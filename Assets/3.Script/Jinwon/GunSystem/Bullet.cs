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
        rb.velocity = transform.forward * bulletSpeed; // Bullet ������ �̵�
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision) // �浹 ó��
    {
        if (!collision.collider.CompareTag("Player")) // ����
        {
            if (collision.collider.GetComponent<Damagable>()) // ������ ���� �� ������ ȣ�� (Damageble�� ���� �׽�Ʈ�� �ۼ��س��� ��ũ��Ʈ, ��ĥ�� ����)
            {
                collision.collider.GetComponent<Damagable>().TakeDamage(bulletDamage);
                //Debug.Log($"{collision.collider.name}���� {bulletDamage}�� ������ ����");
            }

            Vector3 hitDirection = (collision.contacts[0].point - transform.position).normalized;


            if (!collision.collider.CompareTag("Wall")) // ��, �ٴڿ��� ������ ���� �ʿ�
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
