using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Recorder : MonoBehaviour
{
    public static Recorder instance;

    // Cameras
    [Header("Camera Object")]
    [SerializeField] private GameObject cameras;
    private CinemachineFreeLook bulletCamera;

    // Datas
    private Transform enemyTransform;
    private Vector3 bulletDirection;

    private GameObject player;
    private GameObject enemy;

    public GameObject bulletPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        player = GameObject.FindGameObjectWithTag("Player");

        bulletCamera = cameras.transform.Find("Bullet Camera").GetComponent<CinemachineFreeLook>();
    }

    public void UpdateData(GameObject enemy, Vector3 bulletDirection)
    {
        Debug.Log("UPDATE DATA");

        this.enemy = enemy;
        this.enemyTransform = enemy.transform;
        this.bulletDirection = bulletDirection;
    }

    public void Replay()
    {
        UIManager.instance.TurnOffUI();
        player.GetComponent<CharacterMovement>().canMove = false;

        // �� ��ġ ����
        Instantiate(enemy, enemyTransform.position, Quaternion.identity);

        // �Ѿ� �߻�
        GameObject endBullet = Instantiate(bulletPrefab, player.GetComponent<ShootTest>().shootStartPoint.position, Quaternion.identity);
        endBullet.transform.forward = bulletDirection;

        // ī�޶� �Ѿ� ����
        bulletCamera.Follow = endBullet.transform;
        bulletCamera.LookAt = endBullet.transform;
        bulletCamera.gameObject.SetActive(true);
    }
}
