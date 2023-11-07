using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatGPT_algorithms : MonoBehaviour
{
    #region 476�簢��
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

        count = CountRectangles(rectangles, targetWidth, targetHeight);
        Debug.Log("������ ����� ��: " + count);
    }

    //rectangles -> ����Ʈ�� ����� �簢�� ������ ��
    int CountRectangles(List<Rectangle> rectangles, int targetWidth, int targetHeight)
    {
        //���� ������� �簢���� ���� ������
        //numRectangles -> ���� ����ϰ� �ִ� �簢���� ����
        int numRectangles = rectangles.Count;
        //currentCombo -> ���� ������ �� �簢�� ���� ���õ� ������ ����
        int[] currentCombo = new int[numRectangles];

        int count = 0;//����� ��(����)

        while (currentCombo[numRectangles - 1] <= rectangles[numRectangles - 1].height)
        //currentCombo -> �迭. �� �簢�� �������� ���õ� ������ �����ϴ� ����
        //currentCombo[numRectangles - 1] -> ���� ������ ������ �簢�� ������ ���� ���õ� ����
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
                count++;//�κ��丮�� ��ĭ ���� �� ä������ �� count + 1
            }

            int currentIndex = 0; //���� �ݺ��� ������ ó������ �簢�� ������ �ε���
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
