using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_Image : MonoBehaviour
{
    /* 사각형들 정보
    * 1. 넓이
    * 2. 모양 -> 필요 없을 수도 있음 width랑 height를 나눠서 계산하면
    * 
    * 넓이 - int enum string
    */
    public List<Test_Image> Nemo = new List<Test_Image>();

    public int width;
    public int height;
    public int area;

    public Test_Image(int _width, int _height, int _area)
    {
        width = _width;
        height = _height;
        area = _area;
    }

    private void Start()
    {
        //Nemo.Add(new Test_Image());
    }
}
