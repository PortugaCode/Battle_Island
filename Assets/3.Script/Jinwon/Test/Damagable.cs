using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    private float health = 100.0f; // �׽�Ʈ ü��

    public void TakeDamage(float damage) // �׽�Ʈ�� ������ �Դ� �޼���
    {
        health -= damage;

        if (health <= 0)
        {
            Recorder.instance.Replay();
            Destroy(gameObject);
        }
    }
}
