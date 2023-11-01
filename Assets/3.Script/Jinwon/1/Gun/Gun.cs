using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage;
    public float fireRate; // �д� �߻� �� (RPM)
    public int magSize;

    public virtual void Shoot() // �߻�
    {
        
    }

    public virtual void Reload() // ������
    {
        
    }
}
