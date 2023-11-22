using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    FullScreenMode screenMode;
    public Dropdown resolutionDrop;
    public Dropdown frameListDrop;
    public Toggle fullscreenButton;
    List<(int, int)> resolutionList = new List<(int, int)>() { (960, 540), (1280, 720), (1600, 900), (1920, 1080), (2560, 1440), (3840, 2160) };
    List<int> frameList = new List<int>() { 30, 60, 120, 144, 165, 240, 0 };
    public int resolutionNum;
    public int frameNum;

    public GameObject option;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Debug.Log(Screen.currentResolution);
        resolutionDrop.options.Clear();
        frameListDrop.options.Clear();

        int optionNum = 0;
        for (int i = 0; i < resolutionList.Count; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = resolutionList[i].Item1.ToString() + " x " + resolutionList[i].Item2.ToString();
            resolutionDrop.options.Add(option);

            if (resolutionList[i].Item1 == Screen.width && resolutionList[i].Item2 == Screen.height)
            {
                resolutionDrop.value = optionNum;
            }

            optionNum++;
        }
        resolutionDrop.RefreshShownValue();

        int frameOptionNum = 0;
        for (int i = 0; i < resolutionList.Count; i++)
        {
            Dropdown.OptionData frameOption = new Dropdown.OptionData();
            frameOption.text = frameList[i].ToString() + "hz";
            frameListDrop.options.Add(frameOption);

            if (frameList[i] == Screen.currentResolution.refreshRate)
            {
                frameListDrop.value = frameOptionNum;
            }

            frameOptionNum++;
        }
        frameListDrop.RefreshShownValue();

        fullscreenButton.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void DropboxFrameChange(int x)
    {
        frameNum = x;
    }

    public void Fullscreen(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void ApplyButton()
    {
        Screen.SetResolution(resolutionList[resolutionNum].Item1, resolutionList[resolutionNum].Item2, screenMode, frameList[frameNum]);
        Debug.Log("현재 화면 상태" + Screen.currentResolution);
    }
}
