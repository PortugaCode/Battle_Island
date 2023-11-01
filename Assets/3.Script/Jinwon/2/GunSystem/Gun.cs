using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage; // 데미지
    //public float fireRate; // 분당 발사 수 (RPM)
    public float coolDown; // 발사 쿨타임
    public int magSize; // 탄창 용량
    public bool canShoot;

    public virtual void Shoot() // 발사
    {
        
    }

    public virtual void Reload() // 재장전
    {
        
    }
}
