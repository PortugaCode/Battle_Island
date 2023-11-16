using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLight : MonoBehaviour
{
    public GameObject lighting;
    public bool isNight = false;

    private void Update()
    {
        //Merge�Ҷ� �ð��� ���� ���ε� �Ѵ°ɷ� ��������
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
