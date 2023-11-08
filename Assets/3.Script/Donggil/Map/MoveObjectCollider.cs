using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectCollider : MonoBehaviour
{
    public GameObject destination;
    public GameObject mapScale;

    private void Start()
    {
        transform.position = destination.transform.position;
    }
}
