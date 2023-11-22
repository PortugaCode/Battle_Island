using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBGM : MonoBehaviour
{
    private DayController dayCon;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip morning;
    [SerializeField] private AudioClip night;

    private void Start()
    {
        dayCon = FindObjectOfType<DayController>();
    }

    private void Update()
    {
        if(dayCon.timeofDay < 14.0f)
        {
            NowNight();
        }
        else
        {
            NowMorning();
        }
    }

    private void NowMorning()
    {
        audioSource.Stop();
        audioSource.clip = morning;
        audioSource.Play();
    }

    private void NowNight()
    {
        audioSource.Stop();
        audioSource.clip = night;
        audioSource.Play();
    }
}
