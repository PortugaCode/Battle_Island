using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    private float health = 100.0f; // 테스트 체력

    public void TakeDamage(float damage) // 테스트용 데미지 입는 메서드
    {
        health -= damage;

        if (health <= 0)
        {
            Recorder.instance.Replay();
            Destroy(gameObject);
        }
    }
}
