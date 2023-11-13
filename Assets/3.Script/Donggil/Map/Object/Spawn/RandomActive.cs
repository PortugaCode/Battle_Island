using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomActive : MonoBehaviour
{
    public GameObject[] randomObject;


    private void Start()
    {
        RandomActivated();
    }

    private void RandomActivated()
    {
        bool active;

        for (int i = 0; i < randomObject.Length; i++)
        {
            active = Random.value > 0.5f;
            if (active)
            {
                randomObject[i].SetActive(true);
            }
            else
            {
                randomObject[i].SetActive(false);
            }
        }
    }
}
