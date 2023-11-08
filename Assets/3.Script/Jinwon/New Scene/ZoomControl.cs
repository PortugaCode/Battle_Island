using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ZoomControl : MonoBehaviour
{
    // Components
    private Animator animator;

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
        // [�ִϸ����� �Ҵ�]
        TryGetComponent(out animator);

        // [�÷��̾� ������Ʈ �Ҵ�]
        player = GameObject.FindGameObjectWithTag("Player");

        // [ī�޶� ������Ʈ �Ҵ�]
        normalCamera = cameras.transform.Find("Normal Camera").GetComponent<CinemachineFreeLook>();
        firstPersonCamera = cameras.transform.Find("First Person Camera").GetComponent<CinemachineVirtualCamera>();
        thirdPersonCamera = cameras.transform.Find("Third Person Camera").GetComponent<CinemachineFreeLook>();

        // [�Ҵ� �ʱ�ȭ�� ��� �� �Ҵ�]
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
        // [�÷��̾� �� ��Ȱ��ȭ]
        player.transform.Find("Model").gameObject.SetActive(false);

        // [ī�޶� ȸ���� ����ȭ]
        firstPersonCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = normalCamera.m_XAxis.Value;
        firstPersonCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value = 2.0f;
        firstPersonCamera.gameObject.SetActive(true);

        // [1��Ī UI ���]
        UIManager.instance.FirstPersonRifleCrosshair(true);
    }

    public void First_ZoomOut()
    {
        // [ī�޶� ȸ���� ����ȭ]
        normalCamera.m_XAxis.Value = firstPersonCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value;
        normalCamera.m_YAxis.Value = 0.75f;
        firstPersonCamera.gameObject.SetActive(false);

        StartCoroutine(SetActivePlayerModel());

        // [1��Ī UI ���]
        UIManager.instance.FirstPersonRifleCrosshair(false);
    }

    public void Third_ZoomIn()
    {
        // [ī�޶� ȸ���� ����ȭ]
        thirdPersonCamera.m_XAxis.Value = normalCamera.m_XAxis.Value;
        thirdPersonCamera.m_YAxis.Value = normalCamera.m_YAxis.Value - 0.25f;
        thirdPersonCamera.gameObject.SetActive(true);

        // [1��Ī UI ���]
        UIManager.instance.ThirdPersonCrosshair(true);

        // [�ִϸ��̼�]
        animator.SetBool("HasGun", true);
        animator.SetTrigger("Aim");
    }

    public void Third_ZoomOut()
    {
        // [ī�޶� ȸ���� ����ȭ]
        normalCamera.m_XAxis.Value = thirdPersonCamera.m_XAxis.Value;
        normalCamera.m_YAxis.Value = thirdPersonCamera.m_YAxis.Value + 0.25f;
        thirdPersonCamera.gameObject.SetActive(false);

        // [1��Ī UI ���]
        UIManager.instance.ThirdPersonCrosshair(false);

        // [�ִϸ��̼�]
        animator.SetBool("HasGun", true);
        animator.SetTrigger("UnAim");
    }

    private IEnumerator SetActivePlayerModel()
    {
        yield return new WaitForSeconds(0.05f);

        // [�÷��̾� �� Ȱ��ȭ]
        player.transform.Find("Model").gameObject.SetActive(true);

    }
}
