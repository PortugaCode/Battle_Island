using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject model;

    public float timeDelay = 5.0f; // ���߱��� �ɸ��� �ð�

    public void StartTimer()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        float timeLeft = timeDelay;

        while (timeLeft > 0) // �ð��� �� �帣��
        {
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        Explode(); // ����
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2.5f);

        foreach (Collider c in colliders)
        {
            if (c.GetComponent<Damagable>())  // ������ ���� �� ������ ȣ�� (Damageble�� ���� �׽�Ʈ�� �ۼ��س��� ��ũ��Ʈ, ��ĥ�� ����)
            {
                c.GetComponent<Damagable>().TakeDamage(100.0f);
                //Debug.Log($"{c.name}���� 100 �� ������ ����");
            }
        }

        model.SetActive(false);

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
