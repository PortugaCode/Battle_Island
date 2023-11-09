using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType // 총 타입
{
    Rifle1,
    Rifle2,
    Sniper1
}

public class Gun : MonoBehaviour
{
    [Header("Player")]
    private GameObject player;
    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject muzzleFlashEffectPrefab;
    private ZoomControl zoomControl;
    private CombatControl combatControl;

    [Header("Both")]
    [SerializeField] protected GunData[] gunDatas;
    protected GunType gunType; // 총 타입
    protected float damage; // 데미지
    protected float coolDown; // 발사 쿨타임
    protected int magSize; // 탄창 용량
    protected bool canShoot; // 발사 가능 여부

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        zoomControl = player.GetComponent<ZoomControl>();
        combatControl = player.GetComponent<CombatControl>();
    }

    public virtual void PlayerShoot() // 플레이어 발사 메서드
    {
        if (canShoot)
        {
            StartCoroutine(PlayerShoot_co());
        }
    }

    private IEnumerator PlayerShoot_co()
    {
        float timer = coolDown; // 발사 쿨타임

        canShoot = false;

        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f)); // 화면 중앙 (크로스헤어 위치)에 Ray 쏘기
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            // Bullet 생성
            if (combatControl.isFirstPerson) // 1인칭 시점일 때 카메라 조금 앞에서 발사
            {
                Vector3 forwardDirection = (raycastHit.point - zoomControl.firstPersonCamera.transform.position).normalized * 0.75f;

                Vector3 muzzleFlashPosition = new Vector3(zoomControl.firstPersonCamera.transform.position.x, zoomControl.firstPersonCamera.transform.position.y - 0.05f, zoomControl.firstPersonCamera.transform.position.z) + forwardDirection;

                GameObject currentBullet = Instantiate(bulletPrefab, zoomControl.firstPersonCamera.transform.position + forwardDirection, Quaternion.identity);
                currentBullet.transform.forward = forwardDirection;
                currentBullet.GetComponent<Bullet>().bulletDamage = damage;
                currentBullet.GetComponent<Bullet>().hit = raycastHit;
                currentBullet.GetComponentInChildren<TrailRenderer>().enabled = false;

                GameObject muzzleFlashEffect = Instantiate(muzzleFlashEffectPrefab, muzzleFlashPosition, Quaternion.identity);
                muzzleFlashEffect.transform.forward = forwardDirection;
                Destroy(muzzleFlashEffect, 0.5f);
            }
            else if (combatControl.isThirdPerson) // 3인칭 시점일 때 총구에서 발사
            {
                GameObject currentBullet = Instantiate(bulletPrefab, muzzleTransform.position, Quaternion.identity);
                currentBullet.transform.forward = raycastHit.point - muzzleTransform.position;
                currentBullet.GetComponent<Bullet>().bulletDamage = damage;
                currentBullet.GetComponent<Bullet>().hit = raycastHit;

                GameObject muzzleFlashEffect = Instantiate(muzzleFlashEffectPrefab, muzzleTransform.position, Quaternion.identity);
                muzzleFlashEffect.transform.forward = raycastHit.point - muzzleTransform.position;
                Destroy(muzzleFlashEffect, 0.5f);
            }
        }

        //
        // 남은 총알 계산 필요
        //

        // 쿨타임 계산
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        canShoot = true;
    }

    public virtual void PlayerReload() // 플레이어 재장전 메서드
    {

    }

    public virtual void EnemyShoot() // 적 AI 발사 메서드
    {

    }

    public virtual void EnemyReload() // 적 재장전 메서드
    {

    }
}
