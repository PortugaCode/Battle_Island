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
        switch(phase)
        {
            case Phase.Phase1:
                return new Vector3 (20f, 1f, 20f);
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

    public GameObject DeadZonePrefabs;

    private Vector3 DeadZonePosition;
    private Vector3 DeadZoneScale;

    [SerializeField] private GameObject Set;                     //��ǥ��ġ ǥ�ø� ���� �� ������Ʈ
    [SerializeField] private BoxCollider mapRange;
    [SerializeField] private MeshRenderer mesh;

    private bool isGameStart = true;

    private void Start()
    {
        StartCoroutine(DeadZoneStart());
    }

    private void Update()
    {
        //mesh.transform.position += new Vector3(-1, 0, -1);
    }


    private IEnumerator DeadZoneStart()
    {
        float map_X = mapRange.bounds.size.x;
        float map_Z = mapRange.bounds.size.z;

        float initRadius = mesh.bounds.size.x / 2;
        Debug.Log("������ : " + initRadius);

        if (isGameStart)
        {
            DeadZonePosition = new Vector3(Random.Range((map_X / 2) * -1, (map_X / 2)), 0, Random.Range((map_Z / 2) * -1, (map_Z / 2)));        //1������ �ڱ��� ��ġ
            
            
            DeadZoneScale = InitPhase(Phase.Phase1);                   //1������ �ڱ��� ������
            GameObject DeadZoneObject = new GameObject("Phase");        //�ڱ��� ������Ʈ ����
            DeadZoneObject.transform.position = Vector3.zero;           //�ڱ��� ������Ʈ �ʱ� ��ġ
            Set.transform.position = DeadZonePosition;                  //1������ �ڱ��� ��ġ 
            DeadZoneObject.transform.localScale = new Vector3(100f, 1f, 100f);
            DeadZonePrefabs.transform.SetParent(DeadZoneObject.transform);

            //�Ÿ� = �ӷ� x �ð�   �ð� = �Ÿ� / �ӷ�   �ӷ� = �Ÿ� / �ð�
            float distance = Vector3.Distance(DeadZoneObject.transform.position, DeadZonePosition);
            Vector3 scaleDistance = DeadZoneScale - DeadZoneObject.transform.localScale;
            float MaxScaleDistance = Mathf.Max(Mathf.Abs(scaleDistance.x), Mathf.Abs(scaleDistance.y), Mathf.Abs(scaleDistance.z));

            float DeadZoneObjectTime = 60f;

            float phaseSpeed = distance / DeadZoneObjectTime;
            float phaseScaleSpeed = MaxScaleDistance / DeadZoneObjectTime;

            yield return new WaitForSeconds(3f);

            while (distance >= 0.001f && DeadZoneObject.transform.localScale.x >= 20.001f && DeadZoneObject.transform.localScale.z >= 20.001f)
            {
                DeadZoneObject.transform.position = Vector3.MoveTowards(DeadZoneObject.transform.position, DeadZonePosition, phaseSpeed * Time.deltaTime);
                DeadZoneObject.transform.localScale -= new Vector3(phaseScaleSpeed * Time.deltaTime, 0, phaseScaleSpeed * Time.deltaTime);
                yield return null;
            }
            //���� ��ǥ������ �ʱ�ȭ
            DeadZoneObject.transform.position = DeadZonePosition;
            DeadZoneObject.transform.localScale = DeadZoneScale;

            float DeadZoneObjectRadius = mesh.bounds.size.x / 2;
            Debug.Log("1������ ������ : " + DeadZoneObjectRadius);


















            float nextRadius = initRadius * 0.1f;
            Vector3 nextPoint = transform.position + Random.insideUnitSphere * DeadZoneObjectRadius;
            DeadZonePosition = nextPoint;
            DeadZoneScale = InitPhase(Phase.Phase2);

            Set.transform.position = DeadZonePosition;

            distance = Vector3.Distance(DeadZoneObject.transform.position, DeadZonePosition);
            scaleDistance = DeadZoneScale - DeadZoneObject.transform.localScale;
            MaxScaleDistance = Mathf.Max(Mathf.Abs(scaleDistance.x), Mathf.Abs(scaleDistance.y), Mathf.Abs(scaleDistance.z));

            float phase2Time = 60f;

            phaseSpeed = distance / phase2Time;
            phaseScaleSpeed = MaxScaleDistance / phase2Time;

            Debug.Log("2������ ���� 5����");
            yield return new WaitForSeconds(5.0f);
            while (distance >= 0.001f && DeadZoneObject.transform.localScale.x >= 10.001f && DeadZoneObject.transform.localScale.z >= 10.001f)
            {
                DeadZoneObject.transform.position = Vector3.MoveTowards(DeadZoneObject.transform.position, DeadZonePosition, phaseSpeed * Time.deltaTime);
                DeadZoneObject.transform.localScale -= new Vector3(phaseScaleSpeed * Time.deltaTime, 0, phaseScaleSpeed * Time.deltaTime);
                yield return null;
            }
            DeadZoneObject.transform.position = DeadZonePosition;
            DeadZoneObject.transform.localScale = DeadZoneScale;

            float phase2Radius = mesh.bounds.size.x / 2;
            Debug.Log("1������ ������ : " + phase2Radius);
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(Set.transform.position, 20f);
    }
}
