using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage;
    public float fireRate; // 분당 발사 수 (RPM)
    public int magSize;

    public virtual void Shoot() // 발사
    {
        
    }

    public virtual void Reload() // 재장전
    {
        
    }
}
