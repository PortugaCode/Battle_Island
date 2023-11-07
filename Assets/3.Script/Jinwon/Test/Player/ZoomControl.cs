using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ZoomControl : MonoBehaviour
{
    // Player
    private GameObject player;

    // Cameras
    [Header("Camera Object")]
    [SerializeField] private GameObject cameras;
    private CinemachineFreeLook normalCamera;
    private CinemachineVirtualCamera firstPersonCamera;
    private CinemachineFreeLook thirdPersonCamera;

    private void Awake()
    {
        // [플레이어 오브젝트 할당]
        player = GameObject.FindGameObjectWithTag("Player");

        // [카메라 컴포넌트 할당]
        normalCamera = cameras.transform.Find("Normal Camera").GetComponent<CinemachineFreeLook>();
        firstPersonCamera = cameras.transform.Find("First Person Camera").GetComponent<CinemachineVirtualCamera>();
        thirdPersonCamera = cameras.transform.Find("Third Person Camera").GetComponent<CinemachineFreeLook>();

        // [할당 초기화된 경우 재 할당]
        if (normalCamera.Follow == null || normalCamera.LookAt == null)
        {
            normalCamera.Follow = player.transform.Find("Normal LookAt");
            normalCamera.LookAt = player.transform.Find("Normal LookAt");
        }
        if (firstPersonCamera.Follow == null)
        {
            firstPersonCamera.Follow = player.transform.Find("Normal LookAt");
        }
        if (thirdPersonCamera.Follow == null || thirdPersonCamera.LookAt == null)
        {
            thirdPersonCamera.Follow = player.transform.Find("Third Person LookAt");
            thirdPersonCamera.LookAt = player.transform.Find("Third Person LookAt");
        }
    }

    public void First_ZoomIn()
    {
        // [플레이어 모델 비활성화]
        player.transform.Find("Model").gameObject.SetActive(false);

        // [카메라 회전값 동기화]
        firstPersonCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = normalCamera.m_XAxis.Value;
        firstPersonCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value = 2.0f;
        firstPersonCamera.gameObject.SetActive(true);
    }

    public void First_ZoomOut()
    {
        // [카메라 회전값 동기화]
        normalCamera.m_XAxis.Value = firstPersonCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value;
        normalCamera.m_YAxis.Value = 0.75f;
        firstPersonCamera.gameObject.SetActive(false);

        StartCoroutine(SetActivePlayerModel());
    }

    public void Third_ZoomIn()
    {
        // [카메라 회전값 동기화]
        thirdPersonCamera.m_XAxis.Value = normalCamera.m_XAxis.Value;
        thirdPersonCamera.m_YAxis.Value = normalCamera.m_YAxis.Value - 0.25f;
        thirdPersonCamera.gameObject.SetActive(true);
    }

    public void Third_ZoomOut()
    {
        // [카메라 회전값 동기화]
        normalCamera.m_XAxis.Value = thirdPersonCamera.m_XAxis.Value;
        normalCamera.m_YAxis.Value = thirdPersonCamera.m_YAxis.Value + 0.25f;
        thirdPersonCamera.gameObject.SetActive(false);
    }

    private IEnumerator SetActivePlayerModel()
    {
        yield return new WaitForSeconds(0.05f);

        // [플레이어 모델 활성화]
        player.transform.Find("Model").gameObject.SetActive(true);

    }
}
