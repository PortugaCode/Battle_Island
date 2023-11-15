using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    /*
     * 2차원 배열로 inventory 칸을 대입하고 칸이 차지가 안되면 0,
     * 차지되면 item ID 숫자 나오게 하기
     */

    private int width;
    private int height;
    private int[,] gridArray;

    public Grid(int width, int height)
    {
        this.width = width;
        this.height = height;

        gridArray = new int[width, height];
        
        for(int x = 0; x < gridArray.GetLength(0); x++)
        {

        }

    }
   
}
