using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyPlane : MonoBehaviour
{
    public GameObject supplyBox;

    [Header("보급상자 나오는 위치(Plane에 있음)")]
    public Transform dropSupplyPosition;

    public float speed = 10.0f;
    Vector3 planePosition;
    Vector3 destination;
    float droptime = 0;
    float gameTime = 0;

    public bool isPlaneSpawn = false;
    public bool setTimeAndDestination = false;


    private void Update()
    {
        if (isPlaneSpawn)
        {
            if(setTimeAndDestination)
            {
                planePosition = transform.position;
                destination = new Vector3(planePosition.x * -1, planePosition.y, planePosition.z * -1);
                droptime = Random.Range(12.0f, 18.0f);
                setTimeAndDestination = false;
                Debug.Log(droptime);
            }
            MovePlane();
        }
    }

    private void MovePlane()
    {

        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(destination - transform.position, Vector3.up);
        DropSupply();

        if (transform.position == destination)
        {
            isPlaneSpawn = false;
            gameTime = 0;
            gameObject.SetActive(false);
        }
    }

    private void DropSupply()
    {
        gameTime += Time.deltaTime;
        if (gameTime > droptime)
        {
            gameTime = 0;
            Instantiate(supplyBox, dropSupplyPosition.position, Quaternion.identity);
        }
    }

}
