using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SurrItemClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //���� ���Կ� �־�߸� �ϴ� ��ũ��Ʈ��

    [SerializeField] private Image image;

    public bool onClickSlot; //Ŭ��

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.cyan;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClickSlot)
        {
            Debug.Log("Ŭ��!");
            onClickSlot = false;
        }
       
    }
}
