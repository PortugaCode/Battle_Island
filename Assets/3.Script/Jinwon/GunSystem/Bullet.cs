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
        if (!collision.collider.transform.root.CompareTag("Player")) // ����
        {
            Vector3 hitDirection = (collision.contacts[0].point - transform.position).normalized;

            if (!collision.collider.transform.root.CompareTag("Wall")) // ��, �ٴڿ��� ������ ���� �ʿ�
            {
                GameObject hitEffect = Instantiate(hitEffectPrefab, collision.contacts[0].point, Quaternion.Euler(hitDirection));
                Destroy(hitEffect, 0.5f);

                if (hit.point != null)
                {
                    GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                    Destroy(bulletHole, 10.0f);
                }
            }

            if (collision.collider.transform.root.CompareTag("Enemy"))
            {
                // ���� �ʿ�
                collision.collider.transform.root.GetComponent<EnemyHealth>().TakeDamage(bulletDamage, hitDirection);
                //collision.collider.GetComponent<Damagable>().TakeDamage(bulletDamage);
                Recorder.instance.UpdateData(collision.collider.gameObject, transform.forward);
            }

            Destroy(gameObject);
        }
    }
}
