using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSupplyPlane : MonoBehaviour
{
    public GameObject plane;

    public Transform PlaneTransform;

    public float radius = 5.0f;

    private void Start()
    {
        Instantiate(plane, new Vector3(300, 80, 300), Quaternion.identity, PlaneTransform);

        PlaneTransform.GetChild(0).gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            PlaneSpawn();
        }
    }


    private void PlaneSpawn()
    {
        int randomAngle;

        randomAngle = Random.Range(0, 360);

        float x = Mathf.Cos(randomAngle * Mathf.Deg2Rad) * radius;
        float z = Mathf.Sin(randomAngle * Mathf.Deg2Rad) * radius;
        Vector3 spawnPosition = new Vector3(x, 80, z);

        PlaneTransform.GetChild(0).transform.position = spawnPosition;


        PlaneTransform.GetChild(0).gameObject.SetActive(true);
    }
}
