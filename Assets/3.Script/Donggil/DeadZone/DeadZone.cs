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
                return 60.0f;
            case Phase.Phase2:
                return 60.0f;
            case Phase.Phase3:
                return 60.0f;
            case Phase.Phase4:
                return 30.0f;
            case Phase.Phase5:
                return 20.0f;
            case Phase.Wait:
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

    public GameObject DeadZonePrefabs;

    public Vector3 DeadZonePosition;
    public Vector3 DeadZoneScale;

    [SerializeField] private GameObject Set;                     //��ǥ��ġ ǥ�ø� ���� �� ������Ʈ
    [SerializeField] private GameObject DeadZoneObject;
    [SerializeField] private BoxCollider mapRange;
    [SerializeField] private MeshRenderer mesh;


    public GameObject player;


    private bool isGameStart = true;                //���� ���� ����
    private bool isDeadZoneMove = false;            //�ڱ��� ������ ����
    private bool isPointComplete = true;            //�������� ������ ����
    private bool isNextDeadZoneMove = false;        //���� �ڱ�����ġ ������ ����
    private bool isSetTime = false;                 //�ð� ������ ����
    private bool isWaitTime = false;                //���ð� ����

    private float gameTime = 0;
    private float DamageTic = 0;
    private float distance;
    public float radius;
    private float limit = 20;
    private Vector3 scaleDistance;


    private void Init()     //����
    {
        float map_X = mapRange.bounds.size.x - limit;
        float map_Z = mapRange.bounds.size.z - limit;

        radius = mesh.bounds.size.x / 2;
        Debug.Log("������ : " + radius);
        DeadZonePosition = new Vector3(Random.Range((map_X / 2) * -1, (map_X / 2)), 0, Random.Range((map_Z / 2) * -1, (map_Z / 2)));        //1������ �ڱ��� ��ġ
        DeadZoneScale = InitPhase(Phase.Phase1);                             //1������ �ڱ��� ������

        DeadZoneObject.transform.position = Vector3.zero;                    //�ڱ��� ������Ʈ �ʱ� ��ġ
        Set.transform.position = DeadZonePosition;                           //1������ �ڱ��� ��ġ 
        Set.transform.localScale = new Vector3(20f, 1f, 20f);
        DeadZoneObject.transform.localScale = new Vector3(100f, 1f, 100f);
        DeadZonePrefabs.transform.SetParent(DeadZoneObject.transform);

        distance = Distance();
        scaleDistance = ScaleDistance();
    }

    private float Distance()        //�߽� �Ÿ� ��� �޼ҵ�
    {
        float dist = Vector3.Distance(DeadZoneObject.transform.position, DeadZonePosition);
        return dist;
    }

    private Vector3 ScaleDistance() //������ �Ÿ� ��� �޼ҵ�
    {
        Vector3 scaleDist = DeadZoneScale - DeadZoneObject.transform.localScale;
        return scaleDist;
    }

    private void Start()
    {
        Debug.Log(SetPhaseTime(Phase.Phase1));
        Init();
        //StartCoroutine(DeadZoneStart());
    }

    int index = 0;      //������ ����

    private void Update()
    {
        if (isGameStart)                        //���� ���۽�
        {
            if (!isSetTime && !isWaitTime)      //�ð��� ���� �ȵǰ� ���ð��� �ƴ� ��
            {
                gameTime = SetPhaseTime(Phase.Wait);                 //���ð� ����
                Debug.Log(SetPhaseTime(Phase.Wait) + "�� ����.");    //�����ð� 
                isSetTime = true;           //�ð� ���� �Ϸ�
                isWaitTime = true;          //���ð� ���� �Ϸ�
            }
            else if (!isSetTime && isWaitTime)  //���� �ð��� ���� �ȵǰ� ���ð��̸�
            {
                gameTime = SetPhaseTime(Phase.Phase1 + index);  //������ �ð� ����
                isSetTime = true;           //�ð� ���� �Ϸ�
                isWaitTime = false;         //���ð� �ƴ�
                isDeadZoneMove = true;      //�ڱ��� ������
            }

            gameTime -= Time.deltaTime;     //�ð� �پ��
            if (gameTime >= -0.01f)         //���� �ð��� 0�ʰ� �Ǹ�
            {
                if (isWaitTime)             //���� ���ð��̸�
                {
                    //���� �ð� ǥ��
                    if (gameTime <= 30.0f && gameTime > 29.99f) Debug.Log("30�� ����");
                    else if (gameTime <= 10.0f && gameTime > 9.99f) Debug.Log("10�� ����");
                }
                else                        //���ð��� �ƴϸ�
                {
                    if (isDeadZoneMove)     //���� �ڱ����� �����̴� ���¸�
                    {
                        MoveDeadZone(Phase.Phase1 + index);     //�ڱ��� ������
                    }

                    if (!isPointComplete)   //���� ���� �ڱ�����ġ�� ���������� �ʴٸ�
                    {
                        Debug.Log($"{index + 1} ������");
                        distance = Distance();              //���� �ڱ��� �߽ɰ� ���� �ڱ��� �߽��� �Ÿ�
                        scaleDistance = ScaleDistance();    //���� �ڱ��� �����ϰ� ���� �ڱ��� ������ ������ �Ÿ�
                        isPointComplete = true;             //���� �ڱ��� ��ġ ���� �Ϸ�
                    }
                }

            }
            else                            //�ð��� 0�ʺ��� �Ʒ��� ��(����) �� �ð��� ���� ��
            {
                if (isWaitTime)              //���� ���ð��̸�
                {
                    isSetTime = false;      //�ð� ���� �ƴ����� ����
                }

                if (isNextDeadZoneMove)     //���� ���� �ڱ����� �����̴� ������ ��
                {
                    MoveNextDeadZone(Phase.Phase1 + index);     //���� �ڱ��� ������
                }
            }
        }

        if (Vector3.SqrMagnitude(DeadZoneObject.transform.position - player.transform.position) > Mathf.Pow(mesh.bounds.size.x / 2, 2))     //�ڱ��� ������ ��� ��
        {
            DamageTic += Time.deltaTime;
            if (DamageTic >= 1.0f)              //1�ʴ� ������ ����
            {
                Damage(Phase.Phase1 + index);   //����� �������� �������� ��������.
                DamageTic = 0;                  //�ð��� �ʱ�ȭ
            }
        }
    }


    private void MoveDeadZone(Phase phase)
    {

        float MaxScaleDistance = Mathf.Max(Mathf.Abs(scaleDistance.x), Mathf.Abs(scaleDistance.y), Mathf.Abs(scaleDistance.z));     //�� ���� �������� �ִ밪 ���ϱ�

        float DeadZoneObjectTime = SetPhaseTime(phase);                     //�ڱ��� �پ��� �ð��� ������ �ð�

        float phaseSpeed = distance / DeadZoneObjectTime;                   //�ӵ� = �Ÿ� / �ð� (�߽� �Ÿ�)
        float phaseScaleSpeed = MaxScaleDistance / DeadZoneObjectTime;      //�ӵ� = �Ÿ� / �ð� (������ �Ÿ�)

        DeadZoneObject.transform.position = Vector3.MoveTowards(DeadZoneObject.transform.position, DeadZonePosition, phaseSpeed * Time.deltaTime);      //�ڱ����� ������� ������

        //(MoveTowards ���� ���� ������ �����Ӱ� �پ��� �ӵ����̰� ���� ����)
        DeadZoneObject.transform.localScale -= new Vector3(phaseScaleSpeed * Time.deltaTime, 0, phaseScaleSpeed * Time.deltaTime);                      //�ڱ����� ������� �پ��


        //�ڱ��� �Ÿ��� �����ڱ��忡 �����߰� �������� ���� �ڱ��� �����ϰ� �����������
        if (Distance() <= 0.001f && DeadZoneObject.transform.localScale.x <= InitPhase(phase).x && DeadZoneObject.transform.localScale.z <= InitPhase(phase).z)
        {
            if (phase == Phase.Phase5) return;      //������ ������� Ż��
            else
            {
                //��ġ�� �������� �� �¾ƶ������� ����
                DeadZoneObject.transform.position = DeadZonePosition;
                DeadZoneObject.transform.localScale = DeadZoneScale;

                //���� �ڱ��� �������� ���� �ڱ��� ������ ���ؼ� ���� �ڱ��� ��ġ ����
                float currentRadius = radius * SetRadiusRatio(phase);
                float nextRadius = radius * SetRadiusRatio(phase + 1);

                Vector3 nextPoint = Set.transform.position + Random.insideUnitSphere * (currentRadius - nextRadius);        //���� �ڱ��� ��ġ�� ��ü ����
                nextPoint.y = 0f;       //��ü�̹Ƿ� ���̸� 0���� �����Ѵ�
                DeadZoneScale = InitPhase(phase + 1);   //���� �ڱ��� ������ ����
                DeadZonePosition = nextPoint;           //���� �ڱ��� ��ġ ����


                isPointComplete = false;                //��ġ �������� �ƴ�
                isDeadZoneMove = false;                 //�ڱ��� �����̰� ���� ����
                isNextDeadZoneMove = true;              //���� �ڱ����� �������� ��
                if (index < 4) index++;                 //������ ���ϴ� ������ 5�� ������ �ȵ�
                Debug.Log("������ ��");
            }
        }
    }


    private void MoveNextDeadZone(Phase phase)
    {
        //Debug.Log("�����ڱ��� ��ġ��?");
        float MaxScaleDistance = Mathf.Max(Mathf.Abs(scaleDistance.x), Mathf.Abs(scaleDistance.y), Mathf.Abs(scaleDistance.z));
        Set.transform.position = Vector3.MoveTowards(Set.transform.position, DeadZonePosition, distance * Time.deltaTime);
        Set.transform.localScale -= new Vector3(MaxScaleDistance * Time.deltaTime, 0, MaxScaleDistance * Time.deltaTime);
        if (Vector3.Distance(Set.transform.position, DeadZonePosition) <= 0.001f && Set.transform.localScale.x <= InitPhase(phase).x && Set.transform.localScale.z <= InitPhase(phase).z)
        {
            Set.transform.position = DeadZonePosition;
            Set.transform.localScale = DeadZoneScale;

            //������ �ڱ��� �����̴� �޼ҵ�� ����

            isNextDeadZoneMove = false;
            isSetTime = false;
            Debug.Log("���� �ڱ��� �Ϸ�");
        }
    }

    public void Damage(Phase phase)
    {
        //HP -= SetDeadZoneDamage(phase);
        Debug.Log(SetDeadZoneDamage(phase) + " ������");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(DeadZoneObject.transform.position, player.transform.position);
    }
}
