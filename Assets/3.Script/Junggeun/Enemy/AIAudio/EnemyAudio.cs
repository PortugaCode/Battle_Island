using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class EnemyAudio : MonoBehaviour
{
    private AudioSource audiomanager;

    [Header("Audio Clip")]
    [SerializeField] private AudioClip Shot;
    [SerializeField] private AudioClip Reload;

    private void Awake()
    {
        TryGetComponent(out audiomanager);
    }

    public void PlayShot()
    {
        audiomanager.PlayOneShot(Shot);
    }

    public void PlayReload()
    {
        audiomanager.PlayOneShot(Reload);
    }
}
