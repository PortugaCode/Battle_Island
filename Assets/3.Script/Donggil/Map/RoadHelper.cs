using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadHelper : MonoBehaviour
{
    public GameObject roadCorner, roadCross, roadT, roadEnd;
    public GameObject[] roadStraights;
    Dictionary<Vector3Int, GameObject> roadDic = new Dictionary<Vector3Int, GameObject>();
    HashSet<Vector3Int> fixRoadCandidates = new HashSet<Vector3Int>();

    

    public List<Vector3Int> GetRoadPositions()
    {
        return roadDic.Keys.ToList();
    }

    public void PlaceStreetPosition(Vector3 startPos, Vector3Int direction, int length)
    {
        var rotation = Quaternion.identity;
        if (direction.z == 0)
        {
            rotation = Quaternion.Euler(0, 90, 0);
        }

        for (int i = 0; i < length; i++)
        {
            var position = Vector3Int.RoundToInt(startPos + direction * i);
            if (roadDic.ContainsKey(position))
            {
                continue;
            }
            var road = Instantiate(roadStraights[Random.Range(0,roadStraights.Length)], position, rotation, transform);
            roadDic.Add(position, road);
            if (i == 0 || i == length - 1)
            {
                fixRoadCandidates.Add(position);
            }
        }
    }

    public void FixRoad()
    {
        foreach (var position in fixRoadCandidates)
        {
            List<Direction> neiborDirections = PlacementHelper.FindNeighbor(position, roadDic.Keys);

            Quaternion rotation = Quaternion.identity;

            if (neiborDirections.Count == 1)
            {
                Destroy(roadDic[position]);
                if (neiborDirections.Contains(Direction.Up))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neiborDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neiborDirections.Contains(Direction.Right))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                roadDic[position] = Instantiate(roadEnd, position, rotation, transform);
            }
            else if (neiborDirections.Count == 2)
            {
                if (neiborDirections.Contains(Direction.Up) && neiborDirections.Contains(Direction.Down)
                    || neiborDirections.Contains(Direction.Right) && neiborDirections.Contains(Direction.Left))
                {
                    continue;
                }
                Destroy(roadDic[position]);
                if (neiborDirections.Contains(Direction.Up) && neiborDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neiborDirections.Contains(Direction.Right) && neiborDirections.Contains(Direction.Up))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neiborDirections.Contains(Direction.Down) && neiborDirections.Contains(Direction.Right))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                roadDic[position] = Instantiate(roadCorner, position, rotation, transform);
            }
            else if (neiborDirections.Count == 3)
            {
                Destroy(roadDic[position]);
                if (neiborDirections.Contains(Direction.Up)
                    && neiborDirections.Contains(Direction.Left)
                    && neiborDirections.Contains(Direction.Down))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neiborDirections.Contains(Direction.Right)
                    && neiborDirections.Contains(Direction.Up)
                    && neiborDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neiborDirections.Contains(Direction.Down)
                    && neiborDirections.Contains(Direction.Right)
                    && neiborDirections.Contains(Direction.Up))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                roadDic[position] = Instantiate(roadT, position, rotation, transform);
            }
            else
            {
                Destroy(roadDic[position]);
                roadDic[position] = Instantiate(roadCross, position, rotation, transform);
            }
        }
    }


}
