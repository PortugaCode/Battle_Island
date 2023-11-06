using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType // 총 타입
{
    Rifle,
    Sniper
}

public class Gun : MonoBehaviour
{
    public GunType gunType; // 총 타입
    public float damage; // 데미지
    //public float fireRate; // 분당 발사 수 (RPM)
    public float coolDown; // 발사 쿨타임
    public int magSize; // 탄창 용량
    public bool canShoot;

    public virtual void Shoot() // 발사 메서드
    {
        
    }

    public virtual void Reload() // 재장전 메서드
    {
        
    }
}
