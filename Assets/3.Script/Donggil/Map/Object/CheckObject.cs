using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObject : MonoBehaviour
{
    [SerializeField] private GameObject gameObj;
    [SerializeField] private float radius = 5.0f;

    [SerializeField] private MapSize scale;
    [SerializeField] private bool isInMap = false;

    [Header("탐지할 레이어 선택 (태그는 Wall)")]
    public Layers layers;

    [Header("오브젝트 비활성화 체크할때 서로 같은 레이어인가")]
    public bool isThislayerSame = false;

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
        int layerMask = 1 << (int)layers;
        Collider[] colliders;
        if (isInMap)        //맵의 자식오브젝트일경우
        {
            colliders = Physics.OverlapSphere(gameObj.transform.position, radius * scale.gameObject.transform.localScale.x, layerMask);
        }
        else
        {
            colliders = Physics.OverlapSphere(gameObj.transform.position, radius, layerMask);
        }

        //만약 레이어 이름이 ~~ 인 콜라이더가 내부에 없을경우
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Wall"))
            {
                if (isThislayerSame)            //먄약 서로의 레이어가 같을 시
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
                else                        //만약 충돌체크하는 레이어가 다를 시
                {
                    if (colliders.Length == 0)
                    {
                        gameObj.SetActive(true);
                    }
                    //만약 레이어 이름이 ~~ 인 콜라이더가 내부에 있을경우
                    else if (colliders.Length > 0)
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
