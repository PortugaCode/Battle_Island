using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRifle : Gun
{
    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject muzzleFlashEffectPrefab;

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
        float timer = coolDown; // �߻� ��Ÿ��

        canShoot = false;

        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f)); // ȭ�� �߾� (ũ�ν���� ��ġ)�� Ray ���
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            // Bullet ����
            if (player.GetComponent<TPSControl>().isFirstPersonView) // 1��Ī ������ �� ī�޶� ���� �տ��� �߻�
            {
                Vector3 forwardDirection = (raycastHit.point - player.GetComponent<TPSControl>().firstPersonCamera.transform.position).normalized * 0.75f;

                Vector3 muzzleFlashPosition = new Vector3(player.GetComponent<TPSControl>().firstPersonCamera.transform.position.x, player.GetComponent<TPSControl>().firstPersonCamera.transform.position.y - 0.05f, player.GetComponent<TPSControl>().firstPersonCamera.transform.position.z) + forwardDirection;

                GameObject currentBullet = Instantiate(bulletPrefab, player.GetComponent<TPSControl>().firstPersonCamera.transform.position + forwardDirection, Quaternion.identity);
                currentBullet.transform.forward = forwardDirection;
                currentBullet.GetComponent<Bullet>().bulletDamage = damage;
                currentBullet.GetComponentInChildren<TrailRenderer>().enabled = false;

                GameObject muzzleFlashEffect = Instantiate(muzzleFlashEffectPrefab, muzzleFlashPosition, Quaternion.identity);
                muzzleFlashEffect.transform.forward = forwardDirection;
                Destroy(muzzleFlashEffect, 0.5f);
            }
            else if (player.GetComponent<TPSControl>().isThirdPersonView) // 3��Ī ������ �� �ѱ����� �߻�
            {
                GameObject currentBullet = Instantiate(bulletPrefab, muzzleTransform.position, Quaternion.identity);
                currentBullet.transform.forward = raycastHit.point - muzzleTransform.position;
                currentBullet.GetComponent<Bullet>().bulletDamage = damage;

                GameObject muzzleFlashEffect = Instantiate(muzzleFlashEffectPrefab, muzzleTransform.position, Quaternion.identity);
                muzzleFlashEffect.transform.forward = raycastHit.point - muzzleTransform.position;
                Destroy(muzzleFlashEffect, 0.5f);
            }
        }

        //
        // ���� �Ѿ� ��� �ʿ�
        //

        // ��Ÿ�� ���
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        canShoot = true;
    }

    public override void Reload()
    {
        // ������ �޼��� �ʿ�
    }
}
