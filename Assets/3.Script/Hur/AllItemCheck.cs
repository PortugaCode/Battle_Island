using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AllItemCheck : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {

        Debug.Log("Ŭ��");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("�ٿ�");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("����");
    }
}
