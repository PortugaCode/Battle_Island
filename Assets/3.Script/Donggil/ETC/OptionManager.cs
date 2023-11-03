using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    [SerializeField] private GameObject OptionPanel;
    [SerializeField] private GameObject GraphicOption;
    [SerializeField] private GameObject AudioOption;

    public void GraphicOptionCon()
    {
        if(!GraphicOption.activeSelf)
        {
            if(!AudioOption.activeSelf)
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
        if(!AudioOption.activeSelf)
        {
            if(!GraphicOption.activeSelf)
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (OptionPanel.activeSelf)
            {
                OptionPanel.SetActive(false);
            }
            else
            {
                OptionPanel.SetActive(true);
            }
        }
    }
}
