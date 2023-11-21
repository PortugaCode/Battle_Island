using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMinimapMove : MonoBehaviour
{
    public GameObject target;

    private Camera camera;
    private void Start()
    {
        camera = GetComponent<Camera>();
    }
    private void FixedUpdate()
    {
        Vector3 position = new Vector3(target.transform.position.x, 15, target.transform.position.z);

        transform.position = position;

    }
}
