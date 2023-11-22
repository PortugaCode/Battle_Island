using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomObject : MonoBehaviour
{
    private float hp = 3;
    private bool isBoom;
    public bool isboom => isBoom;
    private EffectManager effectManager;
    private float explodeRange = 5f;
    [SerializeField] private GameObject BoomSound;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Effect").TryGetComponent(out effectManager);
    }

    public void TakeDamage(float a = 1)
    {
        hp -= a;

        if (hp <= 0 && !isBoom)
        {
            effectManager.boomEffect.transform.position = gameObject.transform.position;
            effectManager.boomEffect.Play();
            isBoom = true;
            Explode();
        }
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRange); // 폭발 범위 콜라이더 검출

        foreach (Collider c in colliders)
        {
            if (c.transform.root.CompareTag("Enemy"))
            {
                Vector3 direcrion = (c.transform.position - gameObject.transform.position).normalized;
                float damage = 0f;

                // [ 거리에 따른 데미지 계산]
                if (explodeRange * 0.75f >= Vector3.Magnitude(c.transform.position - transform.position))
                {
                    damage = 200.0f;
                }
                else if (explodeRange * 0.75f < Vector3.Magnitude(c.transform.position - transform.position))
                {
                    damage = Vector3.Magnitude(c.transform.position - transform.position) * (-200.0f / 3.0f) + (1000.0f / 3.0f);
                }

                c.transform.root.GetComponent<EnemyHealth>().TakeDamage((int)damage, direcrion);
                //Debug.Log($"수류탄 : {c.name}에게 {(int)damage}의 데미지");
            }

            if (c.CompareTag("Player"))
            {
                float damage = 0f;

                // [ 거리에 따른 데미지 계산]
                if (explodeRange * 0.75f >= Vector3.Magnitude(c.transform.position - transform.position))
                {
                    damage = 200.0f;
                }
                else if (explodeRange * 0.75f < Vector3.Magnitude(c.transform.position - transform.position))
                {
                    damage = Vector3.Magnitude(c.transform.position - transform.position) * (-200.0f / 3.0f) + (1000.0f / 3.0f);
                }

                c.GetComponent<CombatControl>().TakeDamage((int)damage);

                //Debug.Log($"수류탄 : {c.name}에게 {(int)damage}의 데미지");
                //Debug.Log($"player health : {c.GetComponent<CombatControl>().playerHealth}");
            }
        }
        GameObject a = Instantiate(BoomSound, gameObject.transform.position, Quaternion.identity);
        Destroy(a, 3f);
        Destroy(gameObject);
    }
}

