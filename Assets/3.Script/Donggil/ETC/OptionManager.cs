using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class OptionManager : MonoBehaviour
{
    public static OptionManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    public GameObject OptionPanel;
    public GameObject OpenLobby;
    [SerializeField] private GameObject GraphicOption;
    [SerializeField] private GameObject AudioOption;

    public bool isTitle = false;

    public void GraphicOptionCon()
    {
        if (!GraphicOption.activeSelf)
        {
            if (!AudioOption.activeSelf)
            {
                GraphicOption.SetActive(true);
            }
            else
            {
                AudioOption.SetActive(false);
                GraphicOption.SetActive(true);
            }
        }
        else
        {
            AudioOption.SetActive(false);
        }
    }

    public void AudioOptionCon()
    {
        if (!AudioOption.activeSelf)
        {
            if (!GraphicOption.activeSelf)
            {
                AudioOption.SetActive(true);
            }
            else
            {
                GraphicOption.SetActive(false);
                AudioOption.SetActive(true);
            }
        }
        else
        {
            GraphicOption.SetActive(false);
        }
    }


    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "JDScene")
        {
            isTitle = false;
        }
        else if (SceneManager.GetActiveScene().name == "Intro")
        {
            isTitle = true;
        }

        if (isTitle)
        {
            OpenLobby.SetActive(false);
        }
        else
        {
            OpenLobby.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isTitle)
        {
            OpenOption();
        }
    }


    public void OpenOption()
    {

        if (OptionPanel.activeSelf)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            OptionPanel.SetActive(false);
            Time.timeScale = 1.0f;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            OptionPanel.SetActive(true);
            Time.timeScale = 0;
        }

    }

    public void CursorVisible()
    {
        if (!isTitle)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void TimeScaleInActive()
    {
        Time.timeScale = 1.0f;
    }
}
