using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody rig;
    //private Transform Target;
    private Vector3 direction;
    private EffectManager effectManager;

    public float speed = 4f;
    public float Damage;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Effect").TryGetComponent(out effectManager);
        TryGetComponent(out rig);
    }


    private void FixedUpdate()
    {
        rig.velocity = transform.forward * speed;
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if(Damage <= 0) collision.gameObject.GetComponent<CombatControl>().TakeDamage(10);

            else collision.gameObject.GetComponent<CombatControl>().TakeDamage(Damage);

            BulletPooling.Instance.Bullets.Enqueue(gameObject);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
            rig.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
        else if(collision.collider.CompareTag("Boom"))
        {
            collision.gameObject.GetComponent<BoomObject>().TakeDamage();



            effectManager.fireEffect.transform.position = gameObject.transform.position;
            effectManager.fireEffect?.Play();
            BulletPooling.Instance.Bullets.Enqueue(gameObject);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
            rig.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
        else if (collision.collider)
        {
            effectManager.fireEffect.transform.position = gameObject.transform.position;
            effectManager.fireEffect?.Play();
            BulletPooling.Instance.Bullets.Enqueue(gameObject);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
            rig.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }

    }
}
