using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip morning;
    [SerializeField] private AudioClip night;



    private void Update()
    {
        //DayControl Time¿¡ µû¶ó NowMornig || NowNight
    }


    public void NowMornig()
    {
        audioSource.Stop();
        audioSource.clip = morning;
        audioSource.Play();
    }

    public void NowNight()
    {
        audioSource.Stop();
        audioSource.clip = night;
        audioSource.Play();
    }
}
