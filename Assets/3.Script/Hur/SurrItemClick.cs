using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SurrItemClick : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    //여긴 슬롯에 있어야만 하는 스크립트임

    [SerializeField] private Image image;
    [SerializeField] private NoticeItem noticeItem;

    public bool Click = false;
    private void Start()
    {
        noticeItem = FindObjectOfType<NoticeItem>();
    }
    public void OnPointerDown(PointerEventData eventData)
    //주변슬롯에 뜬 아이템을 누르면
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            //툴팁
            Debug.Log("툴팁 뿅 : 사용 or 버리기");

            //버리기는 인벤토리 밖에 버려도 버려져야 함
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
