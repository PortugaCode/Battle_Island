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
        List<Vector3Int> blockedPositions = new List<Vector3Int>();
        foreach (var freeSpot in freeEstateSpots)
        {
            if (blockedPositions.Contains(freeSpot.Key))
            {
                continue;
            }
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
                        var halfSize = Mathf.CeilToInt(buildingTypes[i].sizeRequired / 2.0f);
                        List<Vector3Int> tempPositionsBlock = new List<Vector3Int>();
                        if (VerifyingBuildingFits(halfSize, freeEstateSpots, freeSpot, blockedPositions, ref tempPositionsBlock))
                        {
                            blockedPositions.AddRange(tempPositionsBlock);
                            var building = SpawnPrefab(buildingTypes[i].GetPrefabs(), freeSpot.Key, rotation);
                            buildingDic.Add(freeSpot.Key, building);

                            foreach (var pos in tempPositionsBlock)
                            {
                                buildingDic.Add(pos, building);
                            }
                        }
                        Debug.Log(VerifyingBuildingFits(halfSize, freeEstateSpots, freeSpot, blockedPositions, ref tempPositionsBlock));
                    }
                    else
                    {
                        var building = SpawnPrefab(buildingTypes[i].GetPrefabs(), freeSpot.Key, rotation);
                        buildingDic.Add(freeSpot.Key, building);
                    }
                    break;
                }
            }
        }

    }

    private bool VerifyingBuildingFits(int halfSize, Dictionary<Vector3Int, Direction> freeEstateSpots, KeyValuePair<Vector3Int, Direction> freeSpot, List<Vector3Int> blockedPosition, ref List<Vector3Int> tempPositionBlock)
    {
        Vector3Int direction = Vector3Int.zero;
        if (freeSpot.Value == Direction.Down || freeSpot.Value == Direction.Up)
        {
            direction = Vector3Int.right * 10;
        }
        else
        {
            direction = new Vector3Int(0, 0, 1) * 10;
        }

        for (int i = 1; i <= halfSize; i++)
        {
            var pos1 = freeSpot.Key + direction * i;
            var pos2 = freeSpot.Key - direction * i;
            if (!freeEstateSpots.ContainsKey(pos1) || !freeEstateSpots.ContainsKey(pos2)
                || blockedPosition.Contains(pos1) || blockedPosition.Contains(pos2))
            {
                return false;
            }
            tempPositionBlock.Add(pos1);
            tempPositionBlock.Add(pos2);
        }
        return true;
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
