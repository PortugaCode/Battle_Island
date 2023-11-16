using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip shot;

    public bool isWin;
    public bool isGameOver;

    [SerializeField] private GameObject GameOverCanvas;


    [SerializeField] private GameObject[] Winner;
    [SerializeField] private GameObject WinEffectCanvas;
    [SerializeField] private GameObject WinCanvas;
    [SerializeField] private GameObject LoseCanvas;

    public Text RankingText; // ·©Å· #1
    public Text KillCountText; //5 Kill


    private void Awake()
    {
        TryGetComponent(out audioSource);
    }

    private void Update()
    {
        if(isGameOver)
        {
            GameOverCanvas.SetActive(true);
            if(isWin)
            {
                StartCoroutine(WinEffect());
                isGameOver = false;
            }
            else
            {
                LoseCanvas.SetActive(true);
                isGameOver = false;
            }
        }
    }





    private IEnumerator WinEffect()
    {
        WaitForSeconds du = new WaitForSeconds(0.3f);
        WinEffectCanvas.SetActive(true);
        yield return du;
        Winner[0].SetActive(true);
        audioSource.PlayOneShot(shot);
        yield return du;
        Winner[1].SetActive(true);
        audioSource.PlayOneShot(shot);
        yield return du;
        Winner[2].SetActive(true);
        audioSource.PlayOneShot(shot);
        yield return du;
        Winner[3].SetActive(true);
        audioSource.PlayOneShot(shot);
        yield return du;
        WinEffectCanvas.SetActive(false);
        WinCanvas.SetActive(true);
        isWin = false;
    }
}
