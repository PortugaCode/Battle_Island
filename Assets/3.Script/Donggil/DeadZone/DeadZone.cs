using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public enum Phase
    {
        Phase1,
        Phase2,
        Phase3,
        Phase4,
        Phase5
    };

    public Vector3 InitPhase(Phase phase)
    {
        switch (phase)
        {
            case Phase.Phase1:
                return new Vector3(20f, 1f, 20f);
            case Phase.Phase2:
                return new Vector3(10f, 1f, 10f);
            case Phase.Phase3:
                return new Vector3(6f, 1f, 6f);
            case Phase.Phase4:
                return new Vector3(3f, 1f, 3f);
            case Phase.Phase5:
                return new Vector3(0.1f, 1f, 0.1f);
            default:
                return Vector3.zero;
        }
    }
    public float SetPhaseTime(Phase phase)
    {
        switch (phase)
        {
            case Phase.Phase1:
                return 60.0f;
            case Phase.Phase2:
                return 60.0f;
            case Phase.Phase3:
                return 60.0f;
            case Phase.Phase4:
                return 30.0f;
            case Phase.Phase5:
                return 20.0f;
            default:
                return 0;
        }
    }

    public float SetRadiusRatio(Phase phase)
    {
        switch (phase)
        {
            case Phase.Phase1:
                return 0.2f;
            case Phase.Phase2:
                return 0.1f;
            case Phase.Phase3:
                return 0.06f;
            case Phase.Phase4:
                return 0.03f;
            case Phase.Phase5:
                return 0.001f;
            default:
                return 0;
        }
    }

    public GameObject DeadZonePrefabs;

    private Vector3 DeadZonePosition;
    private Vector3 DeadZoneScale;

    [SerializeField] private GameObject Set;                     //목표위치 표시를 위한 빈 오브젝트
    [SerializeField] private GameObject DeadZoneObject;                 //자기장 오브젝트 생성
    [SerializeField] private BoxCollider mapRange;
    [SerializeField] private MeshRenderer mesh;


    public GameObject player;


    private bool isGameStart = true;
    private bool isDeadZoneMove = false;
    private bool isPointComplete = true;
    private bool isNextDeadZoneMove = false;
    private bool isSetTime = false;

    private float gameTime = 0;
    private float distance;
    private float radius;
    private Vector3 scaleDistance;


    private void Init()
    {
        float map_X = mapRange.bounds.size.x;
        float map_Z = mapRange.bounds.size.z;

        radius = mesh.bounds.size.x / 2;
        Debug.Log("반지름 : " + radius);
        DeadZonePosition = new Vector3(Random.Range((map_X / 2) * -1, (map_X / 2)), 0, Random.Range((map_Z / 2) * -1, (map_Z / 2)));        //1페이즈 자기장 위치
        DeadZoneScale = InitPhase(Phase.Phase1);                             //1페이즈 자기장 스케일

        DeadZoneObject.transform.position = Vector3.zero;                    //자기장 오브젝트 초기 위치
        Set.transform.position = DeadZonePosition;                           //1페이즈 자기장 위치 
        Set.transform.localScale = new Vector3(20f, 1f, 20f);
        DeadZoneObject.transform.localScale = new Vector3(100f, 1f, 100f);
        DeadZonePrefabs.transform.SetParent(DeadZoneObject.transform);

        distance = Distance();
        scaleDistance = ScaleDistance();
    }

    private float Distance()
    {
        float dist = Vector3.Distance(DeadZoneObject.transform.position, DeadZonePosition);
        return dist;
    }

    private Vector3 ScaleDistance()
    {
        Vector3 scaleDist = DeadZoneScale - DeadZoneObject.transform.localScale;
        return scaleDist;
    }

    private void Awake()
    {

    }
    private void Start()
    {
        Debug.Log(SetPhaseTime(Phase.Phase1));
        Init();
        //StartCoroutine(DeadZoneStart());
    }

    private void Definition()
    {
        if (!isPointComplete)
        {
            distance = Distance();
            scaleDistance = ScaleDistance();
            isPointComplete = true;
        }
    }

    int index = 0;

    private void Update()
    {
        if (isGameStart)
        {
            if (!isSetTime)
            {
                gameTime = SetPhaseTime(Phase.Phase1 + index);
                isSetTime = true;
                isDeadZoneMove = true;
            }

            gameTime -= Time.deltaTime;
            if (gameTime >= -0.01f)
            {
                if (isDeadZoneMove)
                {
                    MoveDeadZone(Phase.Phase1 + index);
                }

                if (!isPointComplete)
                {
                    Debug.Log($"{index + 1} 페이즈");
                    distance = Distance();
                    scaleDistance = ScaleDistance();
                    isPointComplete = true;
                }

            }
            else
            {
                if (isNextDeadZoneMove)
                {
                    MoveNextDeadZone(Phase.Phase1 + index);
                }
            }
        }

        if (Vector3.Distance(DeadZoneObject.transform.position, player.transform.position) > mesh.bounds.size.x / 2)
        {
            Damage();
        }
    }


    private void MoveDeadZone(Phase phase)
    {

        float MaxScaleDistance = Mathf.Max(Mathf.Abs(scaleDistance.x), Mathf.Abs(scaleDistance.y), Mathf.Abs(scaleDistance.z));

        float DeadZoneObjectTime = SetPhaseTime(phase);

        float phaseSpeed = distance / DeadZoneObjectTime;
        float phaseScaleSpeed = MaxScaleDistance / DeadZoneObjectTime;

        DeadZoneObject.transform.position = Vector3.MoveTowards(DeadZoneObject.transform.position, DeadZonePosition, phaseSpeed * Time.deltaTime);
        DeadZoneObject.transform.localScale -= new Vector3(phaseScaleSpeed * Time.deltaTime, 0, phaseScaleSpeed * Time.deltaTime);

        if (Distance() <= 0.001f && DeadZoneObject.transform.localScale.x <= InitPhase(phase).x && DeadZoneObject.transform.localScale.z <= InitPhase(phase).z)
        {
            DeadZoneObject.transform.position = DeadZonePosition;
            DeadZoneObject.transform.localScale = DeadZoneScale;

            float currentRadius = radius * SetRadiusRatio(phase);
            float nextRadius = radius * SetRadiusRatio(phase + 1);
            Vector3 nextPoint = Set.transform.position + Random.insideUnitSphere * (currentRadius - nextRadius);
            DeadZoneScale = InitPhase(phase + 1);
            DeadZonePosition = nextPoint;



            isPointComplete = false;
            isDeadZoneMove = false;
            isNextDeadZoneMove = true;
            if (index < System.Enum.GetValues(typeof(Phase)).Length) index++;
            Debug.Log("페이즈 끝");
        }
    }


    private void MoveNextDeadZone(Phase phase)
    {
        //Debug.Log("다음자기장 위치는?");
        float MaxScaleDistance = Mathf.Max(Mathf.Abs(scaleDistance.x), Mathf.Abs(scaleDistance.y), Mathf.Abs(scaleDistance.z));
        Set.transform.position = Vector3.MoveTowards(Set.transform.position, DeadZonePosition, distance * Time.deltaTime);
        Set.transform.localScale -= new Vector3(MaxScaleDistance * Time.deltaTime, 0, MaxScaleDistance * Time.deltaTime);
        if (Vector3.Distance(Set.transform.position, DeadZonePosition) <= 0.001f && Set.transform.localScale.x <= InitPhase(phase).x && Set.transform.localScale.z <= InitPhase(phase).z)
        {
            Set.transform.position = DeadZonePosition;
            Set.transform.localScale = DeadZoneScale;

            isNextDeadZoneMove = false;
            isSetTime = false;
            Debug.Log("다음 자기장 완료");
        }
    }

    public void Damage()
    {
            Debug.Log("데미지");
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(DeadZoneObject.transform.position, player.transform.position);
    }

}
