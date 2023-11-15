using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    /*
     * 2���� �迭�� inventory ĭ�� �����ϰ� ĭ�� ������ �ȵǸ� 0,
     * �����Ǹ� item ID ���� ������ �ϱ�
     */

    [SerializeField] private int width, height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform cam;

    private void Start()
    {
        GenerateGrid();
    }
    void GenerateGrid()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y+9), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}"; //Ÿ�� ȣ��

                var isOffset = (x % 2 == 0 && y % 2 !=0) || (x%2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);
            }
        }
        cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
        
    }
   
}
