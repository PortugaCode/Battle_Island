using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    /*
     * 2차원 배열로 inventory 칸을 대입하고 칸이 차지가 안되면 0,
     * 차지되면 item ID 숫자 나오게 하기
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
                spawnedTile.name = $"Tile {x} {y}"; //타일 호출

                var isOffset = (x % 2 == 0 && y % 2 !=0) || (x%2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);
            }
        }
        cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
        
    }
   
}
