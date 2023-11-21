using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SurrItemClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //���� ���Կ� �־�߸� �ϴ� ��ũ��Ʈ��

    [SerializeField] private Image image;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
        image.color = Color.cyan;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white;
    }
}
