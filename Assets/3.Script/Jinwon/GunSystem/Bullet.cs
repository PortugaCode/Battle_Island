using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float gravity = 9.8f;
    public float bulletSpeed = 1.0f;
    public float bulletDamage = 0;

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
        if (!collision.collider.CompareTag("Player")) // �÷��̾� ����
        {
            if (collision.collider.GetComponent<Damagable>()) // ������ ���� �� ������ ȣ�� (Damageble�� ���� �׽�Ʈ�� �ۼ��س��� ��ũ��Ʈ, ��ĥ�� ����)
            {
                collision.collider.GetComponent<Damagable>().TakeDamage(bulletDamage);
                Debug.Log($"{collision.collider.name}���� {bulletDamage}�� ������ ����");
            }

            Destroy(gameObject);
        }
    }
}
