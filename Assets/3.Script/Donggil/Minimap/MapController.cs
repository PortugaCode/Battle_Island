using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject Map;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            MapOpen();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Map.SetActive(false);
        }
    }

    public void MapOpen()
    {
        if (!Map.activeSelf)
        {
            Map.SetActive(true);
        }
        else
        {
            Map.SetActive(false);
        }
    }
}
