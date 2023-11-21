using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawn : MonoBehaviour
{
    public GameObject car;
    public GameObject destroyedCar;
    public GameObject barricate;

    private void Start()
    {
       
        Spawn();
    }



    private void Spawn()
    {

        int percentage = Random.Range(0, 100);

        Vector3 randomAngle = new Vector3(0, Random.Range(0, 360), 0);

        if (percentage > 92)
        {
            Instantiate(car, transform.position + new Vector3(0, 0, 0), Quaternion.Euler(randomAngle), transform);

        }
        else if (percentage < 20)
        {
            Instantiate(destroyedCar, transform.position + new Vector3(0, 2, 0), Quaternion.Euler(randomAngle), transform);

        }
        else if (percentage >= 20 && percentage < 30)
        {
            for (int i = 0; i < Random.Range(1, 3); i++)
            {
                Instantiate(barricate, transform.position + new Vector3(Random.Range(-1, 1) * 3, 0, 0), Quaternion.identity, transform);
            }
        }
    }
}
