using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatGPT_algorithms : MonoBehaviour
{
    #region 476사각형
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

        count = CountRectangles(rectangles, targetWidth, targetHeight);
        Debug.Log("가능한 경우의 수: " + count);
    }

    //rectangles -> 리스트에 저장된 사각형 종류의 수
    int CountRectangles(List<Rectangle> rectangles, int targetWidth, int targetHeight)
    {
        //현재 사용중인 사각형의 수를 추적함
        //numRectangles -> 현재 사용하고 있는 사각형의 종류
        int numRectangles = rectangles.Count;
        //currentCombo -> 현재 조합의 각 사각형 별로 선택된 개수를 저장
        int[] currentCombo = new int[numRectangles];

        int count = 0;//경우의 수(예상)

        while (currentCombo[numRectangles - 1] <= rectangles[numRectangles - 1].height)
        //currentCombo -> 배열. 각 사각형 종류별로 선택된 개수를 저장하는 변수
        //currentCombo[numRectangles - 1] -> 현재 조합의 마지막 사각형 종류에 대해 선택된 개수
        {
            int totalWidth = 0;
            int totalHeight = 0;

            for (int i = 0; i < numRectangles; i++)
            {
                totalWidth += currentCombo[i] * rectangles[i].width;
                totalHeight += currentCombo[i] * rectangles[i].height;
            }

            if (totalWidth == targetWidth && totalHeight == targetHeight)
            {
                count++;//인벤토리가 빈칸 없이 다 채워졌을 때 count + 1
            }

            int currentIndex = 0; //현재 반복문 내에서 처리중인 사각형 종류의 인덱스
            while (currentIndex < numRectangles)
            {
                if (currentCombo[currentIndex] < rectangles[currentIndex].height)
                {
                    currentCombo[currentIndex]++;
                    break;
                }
                else
                {
                    currentCombo[currentIndex] = 0;
                    currentIndex++;
                }
            }

            if (currentIndex == numRectangles)
            {
                break;
            }
        }

        return count;
    }
    #endregion
}
