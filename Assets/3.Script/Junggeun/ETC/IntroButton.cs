using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroButton : MonoBehaviour
{

    public void SelectLevel(string level)
    {
        if(level.Equals("Easy"))
        {
            GameManager.instance.Level = 3;
            Debug.Log($"{GameManager.instance.Level}");
        }
        else if(level.Equals("Normal"))
        {
            GameManager.instance.Level = 5;
            Debug.Log($"{GameManager.instance.Level}");
        }
        else if (level.Equals("Hard"))
        {
            GameManager.instance.Level = 7;
            Debug.Log($"{GameManager.instance.Level}");
        }
    }

    public void GotoIntro()
    {
        SceneManager.LoadScene("Intro");
    }

    public void StartButton()
    {
        SceneManager.LoadScene("JDScene");
    }

    public void ClickSetting()
    {
        OptionManager.Instance.OptionPanel.SetActive(true);
    }
}
