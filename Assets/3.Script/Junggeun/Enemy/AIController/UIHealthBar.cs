using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    [SerializeField] private Image foreGround;
    [SerializeField] private Image backGround;

    private void LateUpdate()
    {
        Vector3 direction = (target.position - Camera.main.transform.position).normalized;
        bool isBehind = Vector3.Dot(direction, Camera.main.transform.forward) <= 0.0f;
        foreGround.enabled = !isBehind;
        backGround.enabled = !isBehind;
        transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
    }


    public void SetHealthBar(float amount)
    {
        float parentWidth = GetComponent<RectTransform>().rect.width;
        float width = parentWidth * amount;
        foreGround.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }
}
