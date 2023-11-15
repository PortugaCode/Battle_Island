using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private new SpriteRenderer renderer;
    [SerializeField] private GameObject highlight;
    //검색하기: unity grid 좌표
    public void Init(bool isOffset)
    {
        renderer.color = isOffset ? offsetColor : baseColor;
    }
    private void OnMouseEnter()
    {
        highlight.SetActive(true);
    }
    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }
}
