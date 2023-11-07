using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Mixer")]
    public AudioMixer masterMixer;
    public AudioMixer BGMMixer;
    public AudioMixer SFXMixer;


    [Header("Audio Slider")]
    public Slider masterSlider;
    public Slider BGMSlider;
    public Slider SFXSlider;



    public void MasterAudioControl()
    {
        float sound = masterSlider.value;

        if (sound == -40.0f) masterMixer.SetFloat("Master", -80);
        else masterMixer.SetFloat("Master", sound);
    }

    public void BGMAudioControl()
    {
        float sound = BGMSlider.value;

        if (sound == -40.0f) BGMMixer.SetFloat("BGM", -80);
        else BGMMixer.SetFloat("BGM", sound);
    }

    public void SFXAudioControl()
    {
        float sound = SFXSlider.value;

        if (sound == -40.0f) SFXMixer.SetFloat("SFX", -80);
        else SFXMixer.SetFloat("SFX", sound);
    }

    public void MuteAll(bool isMute)
    {
        masterMixer.SetFloat("Master", isMute ? -80 : masterSlider.value);
    }

    public void MuteBGM(bool isMute)
    {
        BGMMixer.SetFloat("BGM", isMute ? -80 : BGMSlider.value);
    }

    public void MuteSFX(bool isMute)
    {
        SFXMixer.SetFloat("SFX", isMute ? -80 : SFXSlider.value);
    }
}
