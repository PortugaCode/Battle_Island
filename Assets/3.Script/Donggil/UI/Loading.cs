using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Slider loadingSlider;
    public Text FinLoading;

    public GameObject loadingScene;

    public NavMeshBaker baker;
    public float loadingTime;
    public float startTime;
    public float currentTime;

    private void Start()
    {
        loadingTime = baker.GetComponent<NavMeshBaker>().bakeTime + baker.GetComponent<NavMeshBaker>().ActiveTime;
        loadingSlider.value = 0;
        startTime = Time.time;
        Debug.Log(loadingTime);
    }

    private void Update()
    {
        DoLoading();
    }
    private void DoLoading()
    {
        currentTime = Time.time - startTime;
        if (currentTime < loadingTime)
        {
            loadingSlider.value = (currentTime / loadingTime);
        }
        else if (Input.anyKeyDown)
        {
            EndLoading();
        }
        else if (currentTime >= loadingTime)
        {
            FinLoading.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }

    }

    private void EndLoading()
    {
        Time.timeScale = 1f;
        loadingSlider.value = 1.0f;
        loadingScene.SetActive(false);
    }
}
