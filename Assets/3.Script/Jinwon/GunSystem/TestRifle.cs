using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRifle : Gun
{
    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private GameObject bulletPrefab;

    private GameObject player;

    private void Awake()
    {
        //damage = 50.0f;
        //fireRate = 60.0f;
        //coolDown = 1.0f;
        //magSize = 30;
        canShoot = true;

        player = GameObject.FindGameObjectWithTag("Player");
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

        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f)); // 화면 중앙 (크로스헤어 위치)에 Ray
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            // Bullet 생성

            if (player.GetComponent<TPSControl>().isFirstPersonView)
            {
                GameObject currentBullet = Instantiate(bulletPrefab, player.GetComponent<TPSControl>().firstPersonCamera.transform.position, Quaternion.identity);
                currentBullet.transform.forward = raycastHit.point - player.GetComponent<TPSControl>().firstPersonCamera.transform.position;
                currentBullet.GetComponent<Bullet>().bulletDamage = damage;
            }
            else if (player.GetComponent<TPSControl>().isThirdPersonView)
            {
                GameObject currentBullet = Instantiate(bulletPrefab, muzzleTransform.position, Quaternion.identity);
                currentBullet.transform.forward = raycastHit.point - muzzleTransform.position;
                currentBullet.GetComponent<Bullet>().bulletDamage = damage;
            }
        }

        //
        // 남은 총알 계산
        //

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
