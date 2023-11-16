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

    [Header("Ž���� ���̾� ����")]
    public Layers layers;

    [Header("Ž���� �±� ����")]
    public selectTag tagName;

    [Header("�˻��ϴ� ������Ʈ�� ���� ���� ���̾��ΰ�")]
    public bool isThislayerSame = false;

    [Header("�� ������Ʈ�� �����Ͻ� üũ")]
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
        if (isInMap)        //���� �ڽĿ�����Ʈ�ϰ��
        {
            colliders = Physics.OverlapSphere(gameObj.transform.position, radius * scale.gameObject.transform.localScale.x, layerMask);
        }
        else
        {
            colliders = Physics.OverlapSphere(gameObj.transform.position, radius, layerMask);
        }
        //Debug.Log(tagString);
        //���� ���̾� �̸��� ~~ �� �ݶ��̴��� ���ο� �������
        foreach (Collider col in colliders)
        {
            if (col.CompareTag(tagString))
            {
                if (isThislayerSame)            //�þ� ������ ���̾ ���� ��
                {
                    if (colliders.Length <= 1)      //1�� �ڱ��ڽŵ� �����ϱ� ����
                    {
                        gameObj.SetActive(true);
                    }
                    //���� ���̾� �̸��� ~~ �� �ݶ��̴��� ���ο� �������
                    else if (colliders.Length > 1)
                    {
                        if (UnityEngine.Random.value > 0.7f)
                        {
                            gameObj.SetActive(false);
                        }
                    }
                }
                else                        //���� �浹üũ�ϴ� ���̾ �ٸ� ��
                {
                    if (colliders.Length <= 0)
                    {
                        gameObj.SetActive(true);
                    }
                    //���� ���̾� �̸��� ~~ �� �ݶ��̴��� ���ο� �������
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
