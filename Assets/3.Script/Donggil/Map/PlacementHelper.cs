using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlacementHelper
{
    public static List<Direction> FindNeighbor(Vector3Int position, ICollection<Vector3Int> collection)
    {
        List<Direction> neighborDirection = new List<Direction>();

        if(collection.Contains(position + Vector3Int.right * 10))
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
}
