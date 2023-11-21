using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSupplyPlane : MonoBehaviour
{
    [SerializeField] private GameObject plane;
    [SerializeField] private Transform PlaneTransform;

    //Merge할때 수정예정
    [Header("다음 자기장 게임 오브젝트 넣기")]
    public GameObject layover;

    public float radius = 350.0f;
    public float nextDeadzoneRadius = 0;

    private DeadZone deadzone;
    private float gameTime = 0;
    private float planeSpawnTime = 0;

    public bool isPlaneSpawned = false;

    private void Start()
    {
        deadzone = FindObjectOfType<DeadZone>();
        planeSpawnTime = Random.Range(1, 4);
        Instantiate(plane, new Vector3(300, 80, 300), Quaternion.identity, PlaneTransform);

        PlaneTransform.GetChild(0).gameObject.SetActive(false);
    }

    private void Update()
    {
        nextDeadzoneRadius = layover.transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size.x / 2;
        transform.position = new Vector3(layover.transform.position.x, 80, layover.transform.position.z);

        if (deadzone.isWaitTime)
        {
            gameTime += Time.deltaTime;
            if (gameTime >= planeSpawnTime && !isPlaneSpawned)
            {
                PlaneSpawn();
                gameTime = 0;
                planeSpawnTime = Random.Range(1, 4);
                isPlaneSpawned = true;
            }
        }
    }

    public float CalcMinTime()
    {
        float dropMinTime = (radius - nextDeadzoneRadius) / plane.GetComponent<SupplyPlane>().speed;
        return dropMinTime;
    }

    public float CalcMaxTime()
    {

        float dropMaxTime = (radius + nextDeadzoneRadius) / plane.GetComponent<SupplyPlane>().speed;
        return dropMaxTime;
    }


    private void PlaneSpawn()
    {
        int randomAngle;

        randomAngle = Random.Range(0, 360);

        float x = Mathf.Cos(randomAngle * Mathf.Deg2Rad) * radius;
        float z = Mathf.Sin(randomAngle * Mathf.Deg2Rad) * radius;
        Vector3 spawnPosition = new Vector3(x + layover.transform.position.x, 80, z + layover.transform.position.z);

        CalcMinTime();
        CalcMaxTime();

        PlaneTransform.GetChild(0).transform.position = spawnPosition;

        PlaneTransform.GetChild(0).gameObject.SetActive(true);
        PlaneTransform.GetChild(0).gameObject.GetComponent<SupplyPlane>().setTimeAndDestination = true;
        PlaneTransform.GetChild(0).gameObject.GetComponent<SupplyPlane>().isPlaneSpawn = true;
    }
}
