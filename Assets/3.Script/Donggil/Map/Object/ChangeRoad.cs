using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoad : MonoBehaviour
{
    public bool isRoadInBound;

    public GameObject Road;

    [SerializeField] private GameObject StraightRoad;
    [SerializeField] private GameObject EndRoad;

    private void Update()
    {
        if (isRoadInBound)
        {
            Road.GetComponent<CheckObject>().enabled = false;
            StraightRoad.SetActive(false);
            EndRoad.SetActive(true);

            Vector3 currentAngle = transform.eulerAngles;
            if (Mathf.Approximately(currentAngle.y, 90.0f))
            {
                EndRoad.SetActive(false);
            }

            isRoadInBound = false;
        }
    }
}
