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
    public Transform muzzleTransform;
    [SerializeField] private GameObject muzzleFlashEffectPrefab;
    private ZoomControl zoomControl;
    private CombatControl combatControl;
    private float timer;

    [Header("Both")]
    [SerializeField] protected GunData[] gunDatas;
    public GunType gunType; // �� Ÿ��
    protected float damage; // ������
    protected float coolDown; // �߻� ��Ÿ��
    public int magSize; // źâ �뷮
    public int currentMag; // ���� źâ�� �ִ� �Ѿ� ����
    public bool canShoot; // �߻� ���� ����

    private void Update()
    {
        if (!canShoot && timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (timer < 0)
        {
            canShoot = true;
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        zoomControl = player.GetComponent<ZoomControl>();
        combatControl = player.GetComponent<CombatControl>();
    }

    public virtual void PlayerShoot() // �÷��̾� �߻� �޼���
    {
        if (canShoot && currentMag > 0)
        {
            canShoot = false;
            timer = coolDown;
            currentMag -= 1;

            UIManager.instance.UpdateAmmoText(currentMag); // Test

            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f)); // ȭ�� �߾� (ũ�ν���� ��ġ)�� Ray ���
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
            {
                // Bullet ����
                if (combatControl.isFirstPerson) // 1��Ī ������ �� ī�޶� ���� �տ��� �߻�
                {
                    Vector3 forwardDirection = (raycastHit.point - zoomControl.firstPersonCamera.transform.position).normalized * 0.75f;

                    Vector3 muzzleFlashPosition = new Vector3(zoomControl.firstPersonCamera.transform.position.x, zoomControl.firstPersonCamera.transform.position.y - 0.05f, zoomControl.firstPersonCamera.transform.position.z) + forwardDirection;

                    // ������Ʈ Ǯ��
                    if (ObjectPoolControl.instance.bulletQueue.Count > 0)
                    {
                        GameObject currentBullet = ObjectPoolControl.instance.bulletQueue.Dequeue();
                        currentBullet.transform.position = zoomControl.firstPersonCamera.transform.position + forwardDirection;
                        currentBullet.transform.forward = forwardDirection;
                        currentBullet.GetComponent<Bullet>().bulletDamage = damage;
                        currentBullet.GetComponent<Bullet>().hit = raycastHit;
                        currentBullet.GetComponentInChildren<TrailRenderer>().enabled = false;
                        currentBullet.SetActive(true);
                    }

                    GameObject muzzleFlashEffect = Instantiate(muzzleFlashEffectPrefab, muzzleFlashPosition, Quaternion.identity);
                    muzzleFlashEffect.transform.forward = forwardDirection;
                    Destroy(muzzleFlashEffect, 0.5f);
                }
                else if (combatControl.isThirdPerson) // 3��Ī ������ �� �ѱ����� �߻�
                {
                    // ������Ʈ Ǯ��
                    if (ObjectPoolControl.instance.bulletQueue.Count > 0)
                    {
                        GameObject currentBullet = ObjectPoolControl.instance.bulletQueue.Dequeue();
                        currentBullet.transform.position = muzzleTransform.position;
                        currentBullet.transform.forward = raycastHit.point - muzzleTransform.position;
                        currentBullet.GetComponent<Bullet>().bulletDamage = damage;
                        currentBullet.GetComponent<Bullet>().hit = raycastHit;
                        currentBullet.GetComponentInChildren<TrailRenderer>().enabled = true;
                        currentBullet.SetActive(true);
                    }

                    GameObject muzzleFlashEffect = Instantiate(muzzleFlashEffectPrefab, muzzleTransform.position, Quaternion.identity);
                    muzzleFlashEffect.transform.forward = raycastHit.point - muzzleTransform.position;
                    Destroy(muzzleFlashEffect, 0.5f);
                }
            }
        }
    }

    public virtual void PlayerReload() // �÷��̾� ������ �޼���
    {
        if (InventoryControl.instance.ammo > 0)
        {
            if (InventoryControl.instance.ammo >= magSize - currentMag)
            {
                int needAmmo = magSize - currentMag;

                InventoryControl.instance.ammo -= needAmmo;
                currentMag = magSize;
            }
            else
            {
                currentMag += InventoryControl.instance.ammo;
                InventoryControl.instance.ammo = 0;
            }
        }

        UIManager.instance.UpdateAmmoText(currentMag); // Test
    }

    public virtual void EnemyShoot() // �� AI �߻� �޼���
    {

    }

    public virtual void EnemyReload() // �� ������ �޼���
    {

    }
}
