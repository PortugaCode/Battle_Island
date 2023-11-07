using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    /*
     * 감지 - 1. 넓이 / 2. 모양
     * 넓이가 맞지 않는다면 그 즉시 아이템 줍기 불가
     * 
     * 최소 칸 넓이
     * 최대 칸 넓이
     * 모든 종류의 사각형 넓이
     * 
     * 아이템을 감지 -> 빈칸 검색(테트리스 한줄한줄 검색) -> 감지1.넓이/2.모양 -> 
     */
    struct Rectangle //구조체 - 각 사각형의 구성요소
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

    //덮어야 할 사각형의 가로, 세로
    public int targetWidth = 17;
    public int targetHeight = 28;

    private List<Rectangle> rectangles = new List<Rectangle>();

    private int count = 0;
    private int blank; //빈칸

    void Start()
    {
        // 각 사각형의 크기와 개수 (각 아이템의 넓이)
        rectangles.Add(new Rectangle("x", 1, 3));
        rectangles.Add(new Rectangle("y", 4, 3));
        rectangles.Add(new Rectangle("z", 4, 4));
        rectangles.Add(new Rectangle("a", 2, 2));
        rectangles.Add(new Rectangle("b", 3, 2));
        rectangles.Add(new Rectangle("c", 3, 3));
        rectangles.Add(new Rectangle("d", 1, 2));
        rectangles.Add(new Rectangle("e", 9, 3));

        //count = CountRectangles(rectangles, targetWidth, targetHeight);
        Debug.Log("가능한 경우의 수: " + count);
    }

    
}
