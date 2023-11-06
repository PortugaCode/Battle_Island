using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private GameObject target;
    Vector3 offset;

    private void Awake()
    {
        offset = transform.position - target.transform.position;
    }

    private void FixedUpdate()
    {
        transform.position = target.transform.position + offset;
    }
}
