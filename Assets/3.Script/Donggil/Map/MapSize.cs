using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSize : MonoBehaviour
{
    private Visualizer magnification;
    [SerializeField] private GameObject Road;

    private void Awake()
    {
        magnification = FindObjectOfType<Visualizer>();
    }

    private void Start()
    {
        Invoke("Calc", 1.0f);
    }

    private void Calc()
    {
        //transform.localScale = new Vector3(magnification.CalcSize(Road), 1, magnification.CalcSize(Road));
    }
}
