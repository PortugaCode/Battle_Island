using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRifle : Gun
{
    private void Awake()
    {
        damage = 1.0f;
        //fireRate = 60.0f;
        coolDown = 1.0f;
        magSize = 30;
        canShoot = true;
    }

    public override void Shoot()
    {
        if (canShoot)
        {
            StartCoroutine(Shoot_co());
        }
    }

    private IEnumerator Shoot_co()
    {
        float timer = coolDown;

        canShoot = false;

        // 발사
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f));
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            Debug.Log(raycastHit.point);
        }

        // 쿨다운 계산
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        canShoot = true;
    }

    public override void Reload()
    {
        base.Reload();
    }
}
