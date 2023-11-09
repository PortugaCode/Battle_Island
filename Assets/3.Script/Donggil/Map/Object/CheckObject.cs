using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObject : MonoBehaviour
{
    [SerializeField] private GameObject gameObj;
    [SerializeField] private float radius = 5.0f;



    private void Update()
    {
        IsObjectExist();
    }

    public bool IsObjectExist()
    {
        Collider[] colliders = Physics.OverlapSphere(gameObj.transform.position, radius, 1 << LayerMask.NameToLayer("Donggil"));

        if(colliders.Length == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(gameObj.transform.position, radius);
    }
}
