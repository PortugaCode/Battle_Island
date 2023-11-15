using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObject : MonoBehaviour
{
    [SerializeField] private GameObject gameObj;
    [SerializeField] private float radius = 5.0f;

    [SerializeField] private MapSize scale;
    [SerializeField] private bool isInMap = false;

    private void Start()
    {
        scale = FindObjectOfType<MapSize>();
    }

    private void Update()
    {
        IsObjectExist();
    }


    public void IsObjectExist()
    {
        Collider[] colliders;
        if (isInMap)        //맵의 자식오브젝트일경우
        {
            colliders = Physics.OverlapSphere(gameObj.transform.position, radius * scale.gameObject.transform.localScale.x, 1 << LayerMask.NameToLayer("Donggil"));
            //만약 레이어 이름이 ~~ 인 콜라이더가 내부에 없을경우
            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Wall") || col.CompareTag("Grenade"))
                {
                    if (colliders.Length == 1)      //1은 자기자신도 포함하기 때문
                    {
                        gameObj.SetActive(true);
                    }
                    //만약 레이어 이름이 ~~ 인 콜라이더가 내부에 있을경우
                    else if (colliders.Length > 1)
                    {
                        gameObj.SetActive(false);
                    }
                }
            }
        }
        else
        {
            colliders = Physics.OverlapSphere(gameObj.transform.position, radius, 1 << LayerMask.NameToLayer("Donggil"));
            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Wall") || col.CompareTag("Grenade"))
                {
                    if (colliders.Length == 1)      //1은 자기자신도 포함하기 때문
                    {
                        gameObj.SetActive(true);
                    }
                    //만약 레이어 이름이 ~~ 인 콜라이더가 내부에 있을경우
                    else if (colliders.Length > 1)
                    {
                        gameObj.SetActive(false);
                    }
                }
            }
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (isInMap)
        {
            Gizmos.DrawSphere(gameObj.transform.position, radius * scale.gameObject.transform.localScale.x);
        }
        else
        {
            Gizmos.DrawSphere(gameObj.transform.position, radius);
        }
    }
}
