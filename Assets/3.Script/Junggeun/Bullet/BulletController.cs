using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody rig;
    //private Transform Target;
    private Vector3 direction;

    public float speed = 4f;

    private void Awake()
    {
        TryGetComponent(out rig);
    }

    private void OnEnable()
    {
        rig.AddForce(transform.forward * speed);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            BulletPooling.Instance.Bullets.Enqueue(gameObject);
        }
        else if(collision.collider)
        {
            BulletPooling.Instance.Bullets.Enqueue(gameObject);
        }
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        rig.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
