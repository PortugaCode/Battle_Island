using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_Image : MonoBehaviour
{
    /* �簢���� ����
    * 1. ����
    * 2. ��� -> �ʿ� ���� ���� ���� width�� height�� ������ ����ϸ�
    * 
    * ���� - int enum string
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
