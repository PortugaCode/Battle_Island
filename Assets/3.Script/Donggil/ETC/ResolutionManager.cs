using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    FullScreenMode screenMode;
    public Dropdown resolutionDrop;
    public Toggle fullscreenButton;
    List<Resolution> resolutions = new List<Resolution>();
    int resolutionNum;

    public GameObject option;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate == 60)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }
        resolutionDrop.options.Clear();

        int optionNum = 0;
        foreach (Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + " x " + item.height;
            resolutionDrop.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
            {
                resolutionDrop.value = optionNum;
            }
            optionNum++;
        }
        resolutionDrop.RefreshShownValue();

        fullscreenButton.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void Fullscreen(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void ApplyButton()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }
}
