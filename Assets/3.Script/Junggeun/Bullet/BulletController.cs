using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]private Rigidbody rigb;

    private void Awake()
    {
        TryGetComponent(out rigb);
        rigb.AddForce(Vector3.forward * 10f);
    }
    private void Start()
    {
        
    }
}
