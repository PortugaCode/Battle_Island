using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform canvas; //UI�� �ҼӵǾ� �ִ� �ֻ���� canvas trasnform
    private Transform previousParent; // �ش� ������ ������ �ҼӵǾ� �־��� �θ� transform
    private RectTransform rect; // ui ��ġ ��� ���� recttrasnform
    private CanvasGroup canvasGroup; // ui ���İ��� ��ȣ�ۿ� ��� ���� canvansgroup

    private void Awake()
    {
        canvas = FindObjectOfType < Canvas >().transform;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        previousParent = transform.parent; //�巡�� ������ ���� �ִ� �θ� Trasnform ���� ����

        //���� �巡������ ui�� ȭ���� �ֻ�ܿ� ��µǵ��� �ϱ� ����
        transform.SetParent(canvas); //�θ� ������ canvas�� ����
        transform.SetAsLastSibling(); // ���� �տ� ���̵��� ������ �ڽ����� ����

        //�巡�� ������ ������ �ϳ��� �ƴ� �ڽĵ��� ������ ���� �� �ֱ� ������ Canvas�� ����
        //���İ��� 0.6���� �����ϰ� ���� �浹ó���� ���� �ʵ��� �Ѵ�.
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false; 
    }
    public void OnDrag(PointerEventData eventData)
    {
        //���� ��ũ������ ���콺 ui ��ġ�� ui ��ġ�� ���� (ui�� ���콺 �Ѿƴٴϴ� ����)
        rect.position = eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        /*�巡�� �����ϸ� �θ� canvas�� �����Ǳ� ����
         * �巡�׸� ������ �� �θ� canvas�� ������ ������ �ƴ� ������ ����
         * ����� �ߴٴ� ���̱� ������ �巡�� ������ �ҼӵǾ� �ִ� ������ �������� ������ �̵�
         */
        if (transform.parent == canvas)
        {
            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponent<RectTransform>().position;
        }

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }

}
