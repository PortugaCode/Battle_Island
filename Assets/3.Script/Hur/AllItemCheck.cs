using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AllItemCheck : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {

        Debug.Log("클릭");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("다운");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("엔터");
    }
}
