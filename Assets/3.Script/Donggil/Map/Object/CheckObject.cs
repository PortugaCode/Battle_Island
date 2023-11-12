using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObject : MonoBehaviour
{
    [SerializeField] private GameObject gameObj;
    [SerializeField] private float radius = 5.0f;

    [SerializeField] private MapSize scale;
    [SerializeField] private bool isInMap = false;

    [Header("Ž���� ���̾� ���� (�±״� Wall)")]
    public Layers layers;

    [Header("������Ʈ ��Ȱ��ȭ üũ�Ҷ� ���� ���� ���̾��ΰ�")]
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
        if (isInMap)        //���� �ڽĿ�����Ʈ�ϰ��
        {
            colliders = Physics.OverlapSphere(gameObj.transform.position, radius * scale.gameObject.transform.localScale.x, layerMask);
        }
        else
        {
            colliders = Physics.OverlapSphere(gameObj.transform.position, radius, layerMask);
        }

        //���� ���̾� �̸��� ~~ �� �ݶ��̴��� ���ο� �������
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Wall"))
            {
                if (isThislayerSame)            //�þ� ������ ���̾ ���� ��
                {
                    if (colliders.Length == 1)      //1�� �ڱ��ڽŵ� �����ϱ� ����
                    {
                        gameObj.SetActive(true);
                    }
                    //���� ���̾� �̸��� ~~ �� �ݶ��̴��� ���ο� �������
                    else if (colliders.Length > 1)
                    {
                        gameObj.SetActive(false);
                    }
                }
                else                        //���� �浹üũ�ϴ� ���̾ �ٸ� ��
                {
                    if (colliders.Length == 0)
                    {
                        gameObj.SetActive(true);
                    }
                    //���� ���̾� �̸��� ~~ �� �ݶ��̴��� ���ο� �������
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
