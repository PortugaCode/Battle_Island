using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType // �� Ÿ��
{
    Rifle1,
    Rifle2,
    Sniper1
}

public class Gun : MonoBehaviour
{
    [Header("Player")]
    private GameObject player;
    [SerializeField] public Transform muzzleTransform;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject muzzleFlashEffectPrefab;
    private ZoomControl zoomControl;
    private CombatControl combatControl;

    [Header("Enemy")]
    private float lastFireTime;

    [Header("Both")]
    [SerializeField] public GunData[] gunDatas;
    public GunType gunType; // �� Ÿ��
    protected float damage; // ������
    protected float coolDown; // �߻� ��Ÿ��
    protected int magSize; // źâ �뷮
    protected bool canShoot; // �߻� ���� ����

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        zoomControl = player.GetComponent<ZoomControl>();
        combatControl = player.GetComponent<CombatControl>();
    }

    public virtual void PlayerShoot() // �÷��̾� �߻� �޼���
    {
        if (canShoot)
        {
            StartCoroutine(PlayerShoot_co());
        }
    }

    private IEnumerator PlayerShoot_co()
    {
        float timer = coolDown; // �߻� ��Ÿ��

        canShoot = false;

        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f)); // ȭ�� �߾� (ũ�ν���� ��ġ)�� Ray ���
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            // Bullet ����
            if (combatControl.isFirstPerson) // 1��Ī ������ �� ī�޶� ���� �տ��� �߻�
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
            else if (combatControl.isThirdPerson) // 3��Ī ������ �� �ѱ����� �߻�
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

    public virtual void PlayerReload() // �÷��̾� ������ �޼���
    {

    }

    public virtual void EnemyShoot(AIAgent agent) // �� AI �߻� �޼���
    {
        Fire(agent);
    }

    public virtual void EnemyReload() // �� ������ �޼���
    {

    }


    private void Fire(AIAgent agent)
    {
        if (Time.time >= lastFireTime + agent.Nowgundata.timebetFire)
        {
            lastFireTime = Time.time;

            Shot(agent);
            agent.isShot = true;
        }
    }

    private void Shot(AIAgent agent)
    {

        Debug.Log("�߻�");

        GameObject b = BulletPooling.Instance.Bullets.Dequeue();
        b.gameObject.SetActive(true);
        b.transform.position = agent.CurrentGun_Gun.muzzleTransform.position;
        b.transform.rotation = agent.CurrentGun_Gun.muzzleTransform.rotation;


        agent.FireEffect.transform.position = muzzleTransform.position;
        agent.FireEffect1.transform.position = transform.position;
        agent.FireEffect.Play();
        agent.FireEffect1.Play();
        agent.animator.SetTrigger("Fire");
        agent.enemyAudio.PlayShot();
        agent.FireEffect2.transform.position = agent.hit.point;

        if (agent.hit.collider)
        {
            if (agent.hit.collider.CompareTag("Wall"))
            {
                agent.FireEffect2.Play();
            }
        }

        agent.magAmmo--;


        //GameObject b = MonoBehaviour.Instantiate(agent.Bullet, agent.SelectStartAim.position, agent.SelectStartAim.transform.rotation);


        /*        Vector3 direction = b.transform.position - agent.AimTarget.position;
                direction.Normalize();
                b.transform.forward = direction;
        
                 GameObject light = MonoBehaviour.Instantiate(agent.FireLight, agent.SelectStartAim.position, Quaternion.identity);
        MonoBehaviour.Destroy(light, 0.03f);*/
    }
}
