using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SimpleVisualizer;

public class Visualizer : MonoBehaviour
{
    public LSystemGenerator lSystem;
    List<Vector3> positions = new List<Vector3>();

    public RoadHelper roadHelper;
    public BuildingHelper buildingHelper;

    public GameObject Road;
    public GameObject Map;

    public bool isMap = false;

    //차후 수정 예정
    private int length = 8;
    private float angle = 90;

    public int Length
    {
        get
        {
            if (length > 0)
            {
                return length;
            }
            else
            {
                return 1;
            }
        }
        set => length = value;
    }

    private void Awake()
    {
        var sequence = lSystem.GenerateSentence();
        VisualizeSequence(sequence);
        CalcSize(Road, Map, isMap);
    }

    private void VisualizeSequence(string sequence)
    {
        Stack<AgentParameters> savePoints = new Stack<AgentParameters>();
        Vector3 currentPosition = Road.transform.position;

        Vector3 direction = Vector3.forward * 10;
        Vector3 tempPosition = Vector3.zero;

        positions.Add(currentPosition);         //건물배치

        foreach (var letter in sequence)
        {
            EncodingLetters encoding = (EncodingLetters)letter;
            switch (encoding)
            {
                case EncodingLetters.save:
                    savePoints.Push(new AgentParameters
                    {
                        position = currentPosition,
                        direction = direction,
                        length = Length
                    });
                    break;
                case EncodingLetters.load:
                    if (savePoints.Count > 0)
                    {
                        var agentParameter = savePoints.Pop();
                        currentPosition = agentParameter.position;
                        direction = agentParameter.direction;

                        Length = agentParameter.length;
                    }
                    else
                    {
                        throw new System.Exception("저장된 포인트 없음");
                    }
                    break;
                case EncodingLetters.draw:

                    tempPosition = currentPosition;
                    currentPosition += direction * length;
                    roadHelper.PlaceStreetPosition(tempPosition, Vector3Int.RoundToInt(direction), length);
                    Length -= 2;
                    //positions.Add(currentPosition);
                    break;
                case EncodingLetters.turnRight:
                    direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                    break;
                case EncodingLetters.turnLeft:
                    direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                    break;
                default:
                    break;
            }
        }

        roadHelper.FixRoad();
        buildingHelper.PlaceBuildingAroundRoad(roadHelper.GetRoadPositions());
    }


    public void CalcSize(GameObject game, GameObject Map, bool IsMap)
    {
        if (IsMap)
        {
            float x = 0;
            float z = 0;

            float map_x = 0;
            float map_z = 0;

            Bounds totalBounds = new Bounds();
            foreach (Collider child in game.GetComponentsInChildren<Collider>())
            {
                totalBounds.Encapsulate(child.bounds);
                x = totalBounds.size.x;
                z = totalBounds.size.z;
            }

            foreach (Collider child in Map.GetComponentsInChildren<Collider>())
            {
                totalBounds.Encapsulate(child.bounds);
                map_x = totalBounds.size.x;
                map_z = totalBounds.size.z;
            }
            Debug.Log($"x : {x}");
            Debug.Log($"z : {z}");
            float max = Mathf.Max(x, z);
            float scaleMag = max * 0.01f;
            //float scaleMag = (max - (Mathf.Abs(x - z)) * 0.6f) * 0.01f;

            if (scaleMag < 2.5f)
            {
                Map.transform.localScale = new Vector3(1 * scaleMag, 1, 1 * scaleMag);
            }
            else
            {
                Map.transform.localScale = new Vector3(1 * 2.5f, 1, 1 * 2.5f);
            }

            //Debug.Log($"map_x : {map_x}");
            //Debug.Log($"map_z : {map_z}");
        }
    }

}
