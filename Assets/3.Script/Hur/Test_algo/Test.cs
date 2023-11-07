using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    /*
     * ���� - 1. ���� / 2. ���
     * ���̰� ���� �ʴ´ٸ� �� ��� ������ �ݱ� �Ұ�
     * 
     * �ּ� ĭ ����
     * �ִ� ĭ ����
     * ��� ������ �簢�� ����
     * 
     * �������� ���� -> ��ĭ �˻�(��Ʈ���� �������� �˻�) -> ����1.����/2.��� -> 
     */
    struct Rectangle //����ü - �� �簢���� �������
    {
        public string name;
        public int width;
        public int height;

        public Rectangle(string name, int width, int height)
        {
            this.name = name;
            this.width = width;
            this.height = height;
        }
    }

    //����� �� �簢���� ����, ����
    public int targetWidth = 17;
    public int targetHeight = 28;

    private List<Rectangle> rectangles = new List<Rectangle>();

    private int count = 0;
    private int blank; //��ĭ

    void Start()
    {
        // �� �簢���� ũ��� ���� (�� �������� ����)
        rectangles.Add(new Rectangle("x", 1, 3));
        rectangles.Add(new Rectangle("y", 4, 3));
        rectangles.Add(new Rectangle("z", 4, 4));
        rectangles.Add(new Rectangle("a", 2, 2));
        rectangles.Add(new Rectangle("b", 3, 2));
        rectangles.Add(new Rectangle("c", 3, 3));
        rectangles.Add(new Rectangle("d", 1, 2));
        rectangles.Add(new Rectangle("e", 9, 3));

        //count = CountRectangles(rectangles, targetWidth, targetHeight);
        Debug.Log("������ ����� ��: " + count);
    }

    
}
