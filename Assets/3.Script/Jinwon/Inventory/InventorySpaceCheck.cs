using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySpaceCheck : MonoBehaviour
{
    private int[,] inventory = new int[17, 28];

    private int maxX = 17;
    private int maxY = 28;

    public int currentItemX;
    public int currentItemY;

    private int place_x = -1; // �������� ���� �� �ִ� ��ġ x
    private int place_y = -1; // �������� ���� �� �ִ� ��ġ y

    private bool haveToTurn = false; // ȸ���ؾ� �ϴ°�

    private void Start()
    {
        inventory = GetComponent<InventoryController_C>().array;
    }

    public int[] CheckBoard(int width, int height)
    {
        if (inventory == null)
        {
            inventory = GetComponent<InventoryController_C>().array;
        }

        currentItemX = width;
        currentItemY = height;

        int[] xy = new int[2];

        for (int i = 0; i < maxY; i++)
        {
            for (int j = 0; j < maxX; j++)
            {
                if (i + currentItemY < maxY && j + currentItemX < maxX)
                {
                    if (inventory[j, i] == 0)
                    {
                        if (CheckSpace(j, i))
                        {
                            place_x = j;
                            place_y = i;
                            xy[0] = place_x;
                            xy[1] = place_y;
                            return xy;
                        }
                    }
                }
            }
        }

        int temp = currentItemX;
        currentItemX = currentItemY;
        currentItemY = temp;

        for (int i = 0; i < maxY; i++)
        {
            for (int j = 0; j < maxX; j++)
            {
                if (i + currentItemY < maxY && j + currentItemX < maxX)
                {
                    if (inventory[j, i] == 0)
                    {
                        if (CheckSpace(j, i))
                        {
                            place_x = j;
                            place_y = i;
                            xy[0] = place_x;
                            xy[1] = place_y;
                            return xy;
                        }
                    }
                }
            }
        }

        return xy;
    }

    private bool CheckSpace(int i, int j)
    {
        for (int k = 0; k < currentItemX; k++)
        {
            for (int l = 0; l < currentItemY; l++)
            {
                if (inventory[i + k, j + l] != 0)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
