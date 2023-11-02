using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody rig;
    private Transform player;
    private Vector3 direction;

    public float speed = 4f;

    private void Awake()
    {
        TryGetComponent(out rig);
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player);

        direction = player.position - transform.position;


        rig.AddForce(direction * speed);
    }

/*    private void Update()
    {

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else if(collision.collider)
        {
            Destroy(gameObject);
        }
        Destroy(gameObject, 2f);
    }
}
