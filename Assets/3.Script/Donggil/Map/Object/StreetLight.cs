using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLight : MonoBehaviour
{
    public GameObject lighting;
    public bool isNight = false;

    private void Update()
    {
        //Merge할때 시간에 따라서 가로등 켜는걸로 수정예정
        if (Input.GetKeyDown(KeyCode.I))
        {
            isNight = !isNight;
        }

        if (isNight)
        {
            lighting.SetActive(true);
        }
        else
        {
            lighting.SetActive(false);
        }
    }
}
