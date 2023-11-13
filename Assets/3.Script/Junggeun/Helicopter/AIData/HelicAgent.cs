using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HelicAgent : MonoBehaviour
{
    [HideInInspector] public HelicStateMachine stateMachine;
    public Transform Player;
    [HideInInspector] public RaycastHit hit;
    [HideInInspector] public RaycastHit bullethit;
    [HideInInspector] public bool isShot;
    [HideInInspector] public bool isShotPattern = false;
    [HideInInspector] public EnemyAudio enemyAudio;

    [Header("Helicopter StartState")]
    public HelicStateID initalState;

    [Header("WallLayer")]
    public LayerMask WallLayer;

    [Header("PlayerLayer")]
    public LayerMask PlayerLayer;

    [Header("TargetAim")]
    public Transform OriginalTarget;
    public Transform OriginalAimTarget;
    public Transform AimTarget;
    public Transform BodyTarget;
    public Transform PositionTarget;

    [Header("FireEffect")]
    public ParticleSystem FireEffect;
    public ParticleSystem FireEffect1;
    public GameObject FireLight;

    private void Awake()
    {
        stateMachine = new HelicStateMachine(this);

        #region 상태추가

        stateMachine.RegsisterState(new HelicRandomMoveState());
        stateMachine.RegsisterState(new HelicChasePlayerState());
        stateMachine.RegsisterState(new HelicShootingState());
        stateMachine.RegsisterState(new HelicBackMoveState());
        stateMachine.RegsisterState(new HelicDieState());

        #endregion


        if (GameObject.FindGameObjectWithTag("Player"))
        {
            GameObject.FindGameObjectWithTag("Player").TryGetComponent(out Player);
        }

        transform.GetChild(0).TryGetComponent(out enemyAudio);

        stateMachine.ChangeState(initalState);
    }

    private void Update()
    {
        stateMachine.Update();
        Physics.Raycast(OriginalTarget.position, OriginalTarget.forward, out bullethit);
        Debug.DrawRay(OriginalTarget.position, OriginalTarget.forward * bullethit.distance, Color.black);


        Physics.Raycast(PositionTarget.position, Vector3.down, out hit);
        Debug.DrawRay(PositionTarget.position, Vector3.down * hit.distance);



        if (isShot)
        {
            StartCoroutine(ShotEffect());
        }
        isShot = false;
    }


    private IEnumerator ShotEffect()
    {

        FireLight.transform.position = OriginalTarget.position;
        FireLight.transform.rotation = OriginalTarget.rotation;
        FireLight.SetActive(true);

        yield return new WaitForSeconds(0.03f);
        FireLight.SetActive(false);
        isShot = false;
    }
}
