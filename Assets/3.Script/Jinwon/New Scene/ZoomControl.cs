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
    public CinemachineFreeLook normalCamera;
    public CinemachineVirtualCamera firstPersonCamera;
    public CinemachineFreeLook thirdPersonCamera;

    // Gun
    private GameObject gun;

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

    public void First_ZoomIn(GameObject currentGun)
    {
        // [�� �𵨸� ��Ȱ��ȭ]
        gun = currentGun;
        gun.transform.SetParent(transform);
        gun.transform.Find("Model").gameObject.SetActive(false);
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localRotation = Quaternion.Euler(Vector3.zero);

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
        // [�� �� Ȱ��ȭ]
        gun.transform.SetParent(GetComponent<CombatControl>().holdGunPivot);
        gun.transform.Find("Model").gameObject.SetActive(true);
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localRotation = Quaternion.Euler(Vector3.zero);

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
        thirdPersonCamera.m_YAxis.Value = normalCamera.m_YAxis.Value - 0.1f;
        thirdPersonCamera.gameObject.SetActive(true);

        // [1��Ī UI ���]
        UIManager.instance.ThirdPersonCrosshair(true);

        // [�ִϸ��̼�]
        animator.SetBool("EquipGun", true);
        animator.SetTrigger("Aim");
    }

    public void Third_ZoomOut()
    {
        // [ī�޶� ȸ���� ����ȭ]
        normalCamera.m_XAxis.Value = thirdPersonCamera.m_XAxis.Value;
        normalCamera.m_YAxis.Value = thirdPersonCamera.m_YAxis.Value + 0.1f;
        thirdPersonCamera.gameObject.SetActive(false);

        // [1��Ī UI ���]
        UIManager.instance.ThirdPersonCrosshair(false);

        // [�ִϸ��̼�]
        animator.SetBool("EquipGun", true);
        animator.SetTrigger("UnAim");
    }

    public void StartLookAround()
    {
        player.GetComponent<CombatControl>().normalCamX = normalCamera.m_XAxis.Value;
        player.GetComponent<CombatControl>().normalCamY = normalCamera.m_YAxis.Value;
    }

    public void EndLookAround()
    {
        normalCamera.m_XAxis.Value = player.GetComponent<CombatControl>().normalCamX;
        normalCamera.m_YAxis.Value = player.GetComponent<CombatControl>().normalCamY;
    }

    private IEnumerator SetActivePlayerModel()
    {
        yield return new WaitForSeconds(0.05f);

        // [�÷��̾� �� Ȱ��ȭ]
        player.transform.Find("Model").gameObject.SetActive(true);
    }
}
