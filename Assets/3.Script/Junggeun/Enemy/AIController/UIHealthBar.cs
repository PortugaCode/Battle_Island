using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private LayerMask WallLayer;

    [SerializeField] private Image foreGround;
    [SerializeField] private Image backGround;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player);
    }

    private void LateUpdate()
    {
        BehindHpBar();

        transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
    }

    private void BehindHpBar()
    {
        Vector3 direction = (target.position - Camera.main.transform.position).normalized;
        bool isBehind = Vector3.Dot(direction, Camera.main.transform.forward) <= 0.0f;

        Vector3 direction2 = player.position - target.position;
        bool isBehind2 = Physics.Raycast(target.position, direction2, Vector3.Distance(player.position, target.position), WallLayer);

        if(isBehind || isBehind2)
        {
            foreGround.enabled = false;
            backGround.enabled = false;
        }
        else
        {
            foreGround.enabled = true;
            backGround.enabled = true;
        }


    }

/*    private void BehindWall()
    {
        Vector3 direction = player.position - target.position;
        bool isBehind;
        if(Physics.Raycast(target.position, direction, Vector3.Distance(player.position, target.position), WallLayer))
        {
            isBehind = true;
        }
        else
        {
            isBehind = false;
        }

        foreGround.enabled = !isBehind;
        backGround.enabled = !isBehind;
    }*/

    public void SetHealthBar(float amount)
    {
        float parentWidth = GetComponent<RectTransform>().rect.width;
        float width = parentWidth * amount;
        foreGround.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }
}
