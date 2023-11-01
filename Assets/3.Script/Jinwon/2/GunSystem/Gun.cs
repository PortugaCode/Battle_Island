using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage; // ������
    //public float fireRate; // �д� �߻� �� (RPM)
    public float coolDown; // �߻� ��Ÿ��
    public int magSize; // źâ �뷮
    public bool canShoot;

    public virtual void Shoot() // �߻�
    {
        
    }

    public virtual void Reload() // ������
    {
        
    }
}
