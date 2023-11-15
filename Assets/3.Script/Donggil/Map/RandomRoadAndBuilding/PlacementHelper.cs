using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlacementHelper
{

    public static List<Direction> FindNeighbor(Vector3Int position, ICollection<Vector3Int> collection)
    {
        List<Direction> neighborDirection = new List<Direction>();

        if (collection.Contains(position + Vector3Int.right * 10))
        {
            neighborDirection.Add(Direction.Right);
        }
        if (collection.Contains(position - Vector3Int.right * 10))
        {
            neighborDirection.Add(Direction.Left);
        }
        if (collection.Contains(position + new Vector3Int(0, 0, 1) * 10))
        {
            neighborDirection.Add(Direction.Up);
        }
        if (collection.Contains(position - new Vector3Int(0, 0, 1) * 10))
        {
            neighborDirection.Add(Direction.Down);
        }

        return neighborDirection;
    }

    internal static Vector3Int GetOffsetFromDirection(Direction direction)
    {
        int distance = 18;
        switch (direction)
        {
            case Direction.Up:
                return new Vector3Int(0, 0, 1) * distance;
            case Direction.Down:
                return new Vector3Int(0, 0, -1) * distance;
            case Direction.Left:
                return Vector3Int.left * distance;
            case Direction.Right:
                return Vector3Int.right * distance;
            default:
                break;
        }
        throw new System.Exception(direction + " 방향이 없음");
    }

    internal static Direction GetReverseDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            default:
                break;
        }
        throw new System.Exception(direction + " 방향이 없음");
    }
}
