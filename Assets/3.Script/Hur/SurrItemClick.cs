using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SurrItemClick : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    //���� ���Կ� �־�߸� �ϴ� ��ũ��Ʈ��

    [SerializeField] private Image image;
    [SerializeField] private NoticeItem noticeItem;

    public bool Click = false;
    private void Start()
    {
        noticeItem = FindObjectOfType<NoticeItem>();
    }
    public void OnPointerDown(PointerEventData eventData)
    //�ֺ����Կ� �� �������� ������
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            //����
            Debug.Log("���� �� : ��� or ������");

            //������� �κ��丮 �ۿ� ������ �������� ��
        }
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.cyan;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white;
    }
}
