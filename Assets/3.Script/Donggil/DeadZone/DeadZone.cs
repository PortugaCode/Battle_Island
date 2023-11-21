using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    /*
     MainCamera -> Rendering -> Culling Mask -> DeadZone 체크 해제
     */
    public enum Phase
    {
        Phase1,
        Phase2,
        Phase3,
        Phase4,
        Phase5,
        Wait
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
                return 60.0f;           //기존 60.0f
            case Phase.Phase2:
                return 60.0f;           //기존 60.0f
            case Phase.Phase3:
                return 60.0f;           //기존 60.0f
            case Phase.Phase4:
                return 30.0f;           //기존 30.0f
            case Phase.Phase5:
                return 20.0f;           //기존 20.0f
            case Phase.Wait:                    //테스트시 5초 테스트 완료시 60초로 변경
                return 5.0f;
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

    public float SetDeadZoneDamage(Phase phase)
    {
        switch (phase)
        {
            case Phase.Phase1:
                return 1f;
            case Phase.Phase2:
                return 2f;
            case Phase.Phase3:
                return 4f;
            case Phase.Phase4:
                return 10f;
            case Phase.Phase5:
                return 20f;
            default:
                return 0;
        }
    }


    private Vector3 DeadZonePosition;
    private Vector3 DeadZoneScale;

    [Header("현재 자기장 오브젝트(자식 부모순으로 넣기)")]
    [Tooltip("자기장 자식 오브젝트")]
    public GameObject DeadZonePrefabs;
    [Tooltip("자기장 부모 오브젝트")]
    public GameObject DeadZoneObject;


    [Header("다음 자기장 오브젝트(부모만 넣기)")]
    public GameObject NextDeadZone;                     //목표위치 표시를 위한 빈 오브젝트

    [Header("자기장 시작 범위(Collider)(콜라이더 포함된 맵 오브젝트 넣기)")]
    [SerializeField] private Collider mapRange;

    [Header("자기장 크기 확인(현재 자기장 자식 오브젝트 넣기)")]
    public MeshRenderer mesh;


    [Header("플레이어와 자기장 거리탐지")]
    public GameObject player;

    public GameObject randomSpawner;
    public List<GameObject> enemyList;

    public LayerMask enemyLayer;


    private bool isGameStart = true;                //게임 시작 여부
    private bool isDeadZoneMove = false;            //자기장 움직임 여부
    private bool isPointComplete = true;            //다음지점 정해짐 여부
    private bool isNextDeadZoneMove = false;        //다음 자기장위치 움직임 여부
    private bool isSetTime = false;                 //시간 정해짐 여부
    private bool isWaitTime = false;                //대기시간 여부

    private float gameTime = 0;
    private float DamageTic = 0;
    private float distance;
    public float radius;
    private float limit = 20;
    private Vector3 scaleDistance;


    private void Init()     //시작
    {
        float map_X = mapRange.bounds.size.x - limit;
        float map_Z = mapRange.bounds.size.z - limit;

        radius = mesh.bounds.size.x / 2;
        Debug.Log("반지름 : " + radius);
        DeadZonePosition = new Vector3(Random.Range((map_X / 2) * -1, (map_X / 2)), 0, Random.Range((map_Z / 2) * -1, (map_Z / 2)));        //1페이즈 자기장 위치
        DeadZoneScale = InitPhase(Phase.Phase1);                             //1페이즈 자기장 스케일

        DeadZoneObject.transform.position = Vector3.zero;                    //자기장 오브젝트 초기 위치
        NextDeadZone.transform.position = DeadZonePosition;                           //1페이즈 자기장 위치 
        NextDeadZone.transform.localScale = new Vector3(20f, 1f, 20f);
        DeadZoneObject.transform.localScale = new Vector3(100f, 1f, 100f);
        DeadZonePrefabs.transform.SetParent(DeadZoneObject.transform);

        distance = Distance();
        scaleDistance = ScaleDistance();
    }

    private float Distance()        //중심 거리 계산 메소드
    {

        float dist = Vector3.Distance(DeadZoneObject.transform.position, DeadZonePosition);
        return dist;
    }

    private Vector3 ScaleDistance() //스케일 거리 계산 메소드
    {
        Vector3 scaleDist = DeadZoneScale - DeadZoneObject.transform.localScale;
        return scaleDist;
    }

    private void Start()
    {
        mapRange = FindObjectOfType<FindRange>().GetComponent<Collider>();
        Debug.Log(SetPhaseTime(Phase.Phase1));
        Init();
        //StartCoroutine(DeadZoneStart());
    }

    int index = 0;      //페이즈 증가

    private void Update()
    {
        if (isGameStart)                        //게임 시작시
        {
            if (!isSetTime && !isWaitTime)      //시간이 지정 안되고 대기시간이 아닐 시
            {
                gameTime = SetPhaseTime(Phase.Wait);                 //대기시간 지정
                Debug.Log(SetPhaseTime(Phase.Wait) + "초 남음.");    //남은시간 
                isSetTime = true;           //시간 지정 완료
                isWaitTime = true;          //대기시간 지정 완료
            }
            else if (!isSetTime && isWaitTime)  //만약 시간이 지정 안되고 대기시간이면
            {
                gameTime = SetPhaseTime(Phase.Phase1 + index);  //페이즈 시간 지정
                isSetTime = true;           //시간 지정 완료
                isWaitTime = false;         //대기시간 아님
                isDeadZoneMove = true;      //자기장 움직임
            }

            gameTime -= Time.deltaTime;     //시간 줄어듦
            if (gameTime >= -0.01f)         //만약 시간이 0초가 되면
            {
                if (isWaitTime)             //만약 대기시간이면
                {
                    //남은 시간 표시
                    if (gameTime <= 30.0f && gameTime > 29.99f) Debug.Log("30초 남음");
                    else if (gameTime <= 10.0f && gameTime > 9.99f) Debug.Log("10초 남음");
                }
                else                        //대기시간이 아니면
                {
                    if (isDeadZoneMove)     //만약 자기장이 움직이는 상태면
                    {
                        MoveDeadZone(Phase.Phase1 + index);     //자기장 움직임
                    }

                    if (!isPointComplete)   //만약 다음 자기장위치가 정해져있지 않다면
                    {
                        Debug.Log($"{index + 1} 페이즈");
                        distance = Distance();              //현재 자기장 중심과 다음 자기장 중심의 거리
                        scaleDistance = ScaleDistance();    //현재 자기장 스케일과 다음 자기장 스케일 사이의 거리
                        isPointComplete = true;             //다음 자기장 위치 지정 완료
                    }
                }

            }
            else                            //시간이 0초보다 아래일 시(음수) 즉 시간이 끝날 시
            {
                if (isWaitTime)              //만약 대기시간이면
                {
                    isSetTime = false;      //시간 지정 아님으로 설정
                }

                if (isNextDeadZoneMove)     //만약 다음 자기장이 움직이는 상태일 시
                {
                    MoveNextDeadZone(Phase.Phase1 + index);     //다음 자기장 움직임
                }
            }
        }



        if (Vector3.SqrMagnitude(DeadZoneObject.transform.position - player.transform.position) > Mathf.Pow(mesh.bounds.size.x / 2, 2))     //자기장 밖으로 벗어날 시
        {
            DamageTic += Time.deltaTime;
            if (DamageTic >= 1.0f)              //1초당 데미지 입음
            {
                Damage(Phase.Phase1 + index);   //페이즈가 지날수록 데미지가 강해진다.
                DamageTic = 0;                  //시간초 초기화
            }
        }

        EnemyDamage(Phase.Phase1 + index);
    }


    private void MoveDeadZone(Phase phase)
    {

        float MaxScaleDistance = Mathf.Max(Mathf.Abs(scaleDistance.x), Mathf.Abs(scaleDistance.y), Mathf.Abs(scaleDistance.z));     //각 축의 스케일중 최대값 구하기

        float DeadZoneObjectTime = SetPhaseTime(phase);                     //자기장 줄어드는 시간은 페이즈 시간

        float phaseSpeed = distance / DeadZoneObjectTime;                   //속도 = 거리 / 시간 (중심 거리)
        float phaseScaleSpeed = MaxScaleDistance / DeadZoneObjectTime;      //속도 = 거리 / 시간 (스케일 거리)

        DeadZoneObject.transform.position = Vector3.MoveTowards(DeadZoneObject.transform.position, DeadZonePosition, phaseSpeed * Time.deltaTime);      //자기장은 등속으로 움직임

        //(MoveTowards 쓰지 않은 이유는 움직임과 줄어드는 속도차이가 나기 때문)
        DeadZoneObject.transform.localScale -= new Vector3(phaseScaleSpeed * Time.deltaTime, 0, phaseScaleSpeed * Time.deltaTime);                      //자기장은 등속으로 줄어듦


        //자기장 거리가 다음자기장에 근접했고 스케일이 다음 자기장 스케일과 근접했을경우
        if (Distance() <= 0.1f &&
            DeadZoneObject.transform.localScale.x - InitPhase(phase).x <= 0.1f &&
            DeadZoneObject.transform.localScale.z - InitPhase(phase).z <= 0.1f)
        {
            if (phase == Phase.Phase5) return;      //마지막 페이즈면 탈출
            else
            {
                //위치와 스케일이 딱 맞아떨어지게 변경
                DeadZoneObject.transform.position = DeadZonePosition;
                DeadZoneObject.transform.localScale = DeadZoneScale;

                //현재 자기장 반지름과 다음 자기장 반지름 구해서 다음 자기장 위치 변경
                float currentRadius = radius * SetRadiusRatio(phase);
                float nextRadius = radius * SetRadiusRatio(phase + 1);

                Vector3 nextPoint = NextDeadZone.transform.position + Random.insideUnitSphere * (currentRadius - nextRadius);        //다음 자기장 위치는 구체 랜덤
                nextPoint.y = 0f;       //구체이므로 높이를 0으로 조정한다
                DeadZoneScale = InitPhase(phase + 1);   //다음 자기장 스케일 지정
                DeadZonePosition = nextPoint;           //다음 자기장 위치 지정


                isPointComplete = false;                //위치 지정상태 아님
                isDeadZoneMove = false;                 //자기장 움직이고 있지 않음
                isNextDeadZoneMove = true;              //다음 자기장이 움직여야 함
                if (index < 4) index++;                 //페이즈 더하는 변수가 5가 넘으면 안됨
                Debug.Log("페이즈 끝");
            }
        }
    }


    private void MoveNextDeadZone(Phase phase)
    {
        //Debug.Log("다음자기장 위치는?");
        float MaxScaleDistance = Mathf.Max(Mathf.Abs(scaleDistance.x), Mathf.Abs(scaleDistance.y), Mathf.Abs(scaleDistance.z));
        NextDeadZone.transform.position = Vector3.MoveTowards(NextDeadZone.transform.position, DeadZonePosition, distance * Time.deltaTime);
        NextDeadZone.transform.localScale -= new Vector3(MaxScaleDistance * Time.deltaTime, 0, MaxScaleDistance * Time.deltaTime);
        if (Vector3.Distance(NextDeadZone.transform.position, DeadZonePosition) <= 0.1f &&
            NextDeadZone.transform.localScale.x - InitPhase(phase).x <= 0.1f && 
            NextDeadZone.transform.localScale.z - InitPhase(phase).z <= 0.1f)
        {
            NextDeadZone.transform.position = DeadZonePosition;
            NextDeadZone.transform.localScale = DeadZoneScale;

            //위까지 자기장 움직이는 메소드와 같음

            isNextDeadZoneMove = false;
            isSetTime = false;
            Debug.Log("다음 자기장 완료");
        }
    }

    public void Damage(Phase phase)
    {
        player.GetComponent<CombatControl>().TakeDamage(SetDeadZoneDamage(phase));
        Debug.Log("HP : " + player.GetComponent<CombatControl>().playerHealth);
    }

    public void EnemyDamage(Phase phase)
    {
        enemyList = randomSpawner.GetComponent<RendomSpawner>().enemyList;
        DamageTic += Time.deltaTime;

        foreach (GameObject enemy in enemyList)
        {
                enemy.transform.root.GetComponent<EnemyHealth>().phase = phase;
            if (Vector3.SqrMagnitude(DeadZoneObject.transform.position - enemy.transform.position) > Mathf.Pow(mesh.bounds.size.x / 2, 2))
            {
                enemy.transform.root.GetComponent<EnemyHealth>().isDeadZone = true;
            }
            else
            {
                enemy.transform.root.GetComponent<EnemyHealth>().isDeadZone = false;
            }
        }

    }

    public float CurrentRadius()                    //실시간 자기장 크기 가져오기
    {
        float rad = mesh.bounds.size.x / 2;
        return rad;
    }

    public Vector3 CurrentDeadZonePosition()        //실시간 자기장 중심좌표 가져오기
    {
        return DeadZoneObject.transform.position;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(DeadZoneObject.transform.position, player.transform.position);
    }
}

