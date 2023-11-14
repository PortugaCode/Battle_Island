using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private GameObject bulletHolePrefab;

    public Vector3 startPostion;

    public float gravity = 9.8f;
    public float bulletSpeed = 1.0f;
    public float bulletDamage = 0;

    public RaycastHit hit;

    private Rigidbody rb;

    private void Awake()
    {
        TryGetComponent(out rb);
    }

    private void OnEnable()
    {
        rb.velocity = transform.forward * bulletSpeed; // Bullet 앞으로 이동
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision) // 충돌 처리
    {
        if (!collision.collider.transform.root.CompareTag("Player")) // 무시
        {
            Vector3 hitDirection = (collision.contacts[0].point - transform.position).normalized;

            if (!collision.collider.transform.root.CompareTag("Wall")) // 벽, 바닥에만 나오게 변경 필요
            {

                if (ObjectPoolControl.instance.hitEffectQueue.Count > 0)
                {
                    GameObject currentHitEffect = ObjectPoolControl.instance.hitEffectQueue.Dequeue();
                    currentHitEffect.transform.position = collision.contacts[0].point;
                    currentHitEffect.transform.rotation = Quaternion.Euler(hitDirection);
                    currentHitEffect.SetActive(true);
                }

                if (hit.point != null)
                {
                    GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                    Destroy(bulletHole, 10.0f);
                }
            }

            if (collision.collider.transform.root.CompareTag("Enemy"))
            {
                // 수정 필요
                collision.collider.transform.root.GetComponent<EnemyHealth>().TakeDamage(bulletDamage, hitDirection);
                //collision.collider.GetComponent<Damagable>().TakeDamage(bulletDamage);
                Recorder.instance.UpdateData(collision.collider.gameObject, startPostion ,transform.forward);
            }

            // 오브젝트 풀링
            gameObject.SetActive(false);
            ObjectPoolControl.instance.bulletQueue.Enqueue(gameObject);
        }
    }
}
