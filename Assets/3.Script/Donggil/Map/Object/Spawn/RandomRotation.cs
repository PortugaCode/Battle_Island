using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    [SerializeField] private int minAngle = 0;
    [SerializeField] private int maxAngle = 360;
    void Start()
    {
        Vector3 angle = new Vector3(0, Random.Range(minAngle, maxAngle), 0);
        transform.rotation *= Quaternion.Euler(angle);
    }
}
