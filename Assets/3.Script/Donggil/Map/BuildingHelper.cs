using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHelper : MonoBehaviour
{
    public HouseType[] buildingTypes;

    public Dictionary<Vector3Int, GameObject> buildingDic = new Dictionary<Vector3Int, GameObject>();

    public void PlaceBuildingAroundRoad(List<Vector3Int> roadPos)
    {
        Dictionary<Vector3Int, Direction> freeEstateSpots = FindFreeSpaceAroundRoad(roadPos);

        foreach (var freeSpot in freeEstateSpots)
        {
            var rotation = Quaternion.identity;
            switch (freeSpot.Value)
            {
                case Direction.Down:
                    rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case Direction.Left:
                    rotation = Quaternion.Euler(0, -90, 0);
                    break;
                case Direction.Right:
                    rotation = Quaternion.Euler(0, 90, 0);
                    break;
                default:
                    break;
            }
            for (int i = 0; i < buildingTypes.Length; i++)
            {
                if (buildingTypes[i].quantity == -1)
                {
                    var building = SpawnPrefab(buildingTypes[i].GetPrefabs(), freeSpot.Key, rotation);
                    buildingDic.Add(freeSpot.Key, building);
                    break;
                }
                if (buildingTypes[i].IsBuildingAvailable())
                {
                    if (buildingTypes[i].sizeRequired > 1)
                    {

                    }
                    else
                    {
                        var building = SpawnPrefab(buildingTypes[i].GetPrefabs(), freeSpot.Key, rotation);
                        buildingDic.Add(freeSpot.Key, building);
                    }
                }
            }
        }
                        
    }

    private GameObject SpawnPrefab(GameObject prefab, Vector3Int position, Quaternion rotation)
    {
        var newBuilding = Instantiate(prefab, position, rotation, transform);
        return newBuilding;
    }

    private Dictionary<Vector3Int, Direction> FindFreeSpaceAroundRoad(List<Vector3Int> roadPos)
    {
        Dictionary<Vector3Int, Direction> freeSpaces = new Dictionary<Vector3Int, Direction>();
        foreach (var position in roadPos)
        {
            var neiborDirections = PlacementHelper.FindNeighbor(position, roadPos);
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (neiborDirections.Contains(direction) == false)
                {
                    var newPos = position + PlacementHelper.GetOffsetFromDirection(direction);
                    if (freeSpaces.ContainsKey(newPos))
                    {
                        continue;
                    }
                    //나중에 수정
                    freeSpaces.Add(newPos, PlacementHelper.GetReverseDirection(direction));
                }
            }
        }
        return freeSpaces;
    }
}
