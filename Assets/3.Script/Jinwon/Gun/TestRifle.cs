using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRifle : Gun
{
    private void Awake()
    {
        damage = 1.0f;
        fireRate = 60.0f;
        magSize = 30;
    }

    public override void Shoot()
    {
        base.Shoot();
    }

    public override void Reload()
    {
        base.Reload();
    }
}
