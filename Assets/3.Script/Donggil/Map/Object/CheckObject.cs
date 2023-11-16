using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObject : MonoBehaviour
{
    public enum selectTag
    {
        Wall = 0,
        Dongil01 = 1,
        Dongil02 = 2
    }

    [SerializeField] private GameObject gameObj;
    [SerializeField] private float radius = 5.0f;

    [SerializeField] private MapSize scale;
    [SerializeField] private bool isInMap = false;

    [Header("탐지할 레이어 선택")]
    public Layers layers;

    [Header("탐지할 태그 선택")]
    public selectTag tagName;

    [Header("검사하는 오브젝트가 서로 같은 레이어인가")]
    public bool isThislayerSame = false;

    [Header("이 오브젝트가 도로일시 체크")]
    public bool isRoad = false;
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
        string tagString = Enum.GetName(typeof(selectTag), tagName);
        Collider[] colliders;
        if (isInMap)        //맵의 자식오브젝트일경우
        {
            colliders = Physics.OverlapSphere(gameObj.transform.position, radius * scale.gameObject.transform.localScale.x, layerMask);
        }
        else
        {
            colliders = Physics.OverlapSphere(gameObj.transform.position, radius, layerMask);
        }
        //Debug.Log(tagString);
        //만약 레이어 이름이 ~~ 인 콜라이더가 내부에 없을경우
        foreach (Collider col in colliders)
        {
            if (col.CompareTag(tagString))
            {
                if (isThislayerSame)            //먄약 서로의 레이어가 같을 시
                {
                    if (colliders.Length <= 1)      //1은 자기자신도 포함하기 때문
                    {
                        gameObj.SetActive(true);
                    }
                    //만약 레이어 이름이 ~~ 인 콜라이더가 내부에 있을경우
                    else if (colliders.Length > 1)
                    {
                        if (UnityEngine.Random.value > 0.7f)
                        {
                            gameObj.SetActive(false);
                        }
                    }
                }
                else                        //만약 충돌체크하는 레이어가 다를 시
                {
                    if (colliders.Length <= 0)
                    {
                        gameObj.SetActive(true);
                    }
                    //만약 레이어 이름이 ~~ 인 콜라이더가 내부에 있을경우
                    else if (colliders.Length > 0)
                    {
                        if (UnityEngine.Random.value > 0.7f)
                        {
                            if (isRoad)
                            {
                                gameObject.GetComponent<ChangeRoad>().isRoadInBound = true;
                            }
                            else
                            {
                                gameObj.SetActive(false);
                            }
                        }
                    }
                }
            }
        }



    }

    private void OnDrawGizmosSelected()
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
