using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject model;

    public float timeDelay = 5.0f; // 폭발까지 걸리는 시간

    public void StartTimer()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        float timeLeft = timeDelay;

        while (timeLeft > 0) // 시간이 다 흐르면
        {
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        Explode(); // 폭발
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2.5f);

        foreach (Collider c in colliders)
        {
            if (c.GetComponent<Damagable>())  // 데미지 받을 수 있으면 호출 (Damageble은 현재 테스트로 작성해놓은 스크립트, 합칠때 수정)
            {
                c.GetComponent<Damagable>().TakeDamage(100.0f);
                //Debug.Log($"{c.name}에게 100 의 데미지 입힘");
            }
        }

        model.SetActive(false);

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
