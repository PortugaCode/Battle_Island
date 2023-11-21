using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayController : MonoBehaviour
{
    [SerializeField] private Light lightControl;
    [SerializeField] private Material Skymat;

    private bool isNight = false;

    [SerializeField, Range(0, 24)] private float timeofDay;
    [SerializeField, Range(0, 24)] private float timeofDay2;
    [SerializeField] private float sunSpeed;

    private void Update()
    {
        timeofDay2 += sunSpeed * Time.deltaTime;
        if (timeofDay2 > 24) timeofDay2 = 0f;

        if (!isNight)
        {
            timeofDay += sunSpeed * Time.deltaTime;
        }
        else if(isNight)
        {
            timeofDay -= sunSpeed * Time.deltaTime;
        }

        if (timeofDay >= 24)
        {
            isNight = true;
        }
        else if(timeofDay <= 0)
        {
            isNight = false;
        }


        DayLight();
    }

    private void DayLight()
    {
        float sunRotation = Mathf.Lerp(0, 360, timeofDay2 / 24);
        lightControl.transform.rotation = Quaternion.Euler(39.179f, sunRotation, -88.845f);

        float timeFraction = timeofDay / 24;
        lightControl.intensity = timeFraction;
        if(lightControl.intensity < 0.2f)
        {
            lightControl.intensity = 0.2f;
        }

        RenderSettings.ambientIntensity = lightControl.intensity;

        Skymat.SetFloat("_Exposure", timeFraction);
    }


}
