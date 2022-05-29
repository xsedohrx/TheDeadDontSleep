using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Time controller for day/night cycle.
/// If you want to change the speed that the day progresses just increase/decrease the multiplier in the inspector.
/// </summary>
public class TimeController : MonoBehaviour
{
    [SerializeField] private float timeMultiplier;
    [SerializeField] private float startHour;
    [SerializeField] private Text timeText;
    [SerializeField] Light sunLight;
    [SerializeField] private float sunRiseHour;
    [SerializeField] private float sunSetHour;

    [SerializeField] private Color dayAmbientLight;
    [SerializeField] private Color nightAmbientLight;
    [SerializeField] private AnimationCurve lightChangeCurve;
    [SerializeField] private float maxSunlightIntensity;
    [SerializeField] private Light moonLight;
    [SerializeField] private float maxMoonlightIntensity;

    private DateTime currentTime;
    private TimeSpan sunRiseTime;
    private TimeSpan sunSetTime;
    
    private void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);

        sunRiseTime = TimeSpan.FromHours(sunRiseHour);
        sunSetTime = TimeSpan.FromHours(sunSetHour);

    }

    private void Update()
    {
        UpdateTimeOfDay();
        RotateSun();
        UpdateLightSettings();
    }

    private void UpdateTimeOfDay() {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);

        if (timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm");
        }
    }

    private void RotateSun() {
        float sunLightRotation;

        if (currentTime.TimeOfDay > sunRiseTime && currentTime.TimeOfDay < sunSetTime)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunRiseTime, sunSetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunRiseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);

        }
        else
        {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunSetTime, sunRiseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunSetTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }

    TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime) {
        TimeSpan diff = toTime - fromTime;
        
        if (diff.TotalSeconds < 0)
        {
            diff += TimeSpan.FromHours(24);
        }
        return diff;
    }

    private void UpdateLightSettings() {
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        sunLight.intensity = Mathf.Lerp(0, maxSunlightIntensity, lightChangeCurve.Evaluate(dotProduct));
        sunLight.intensity = Mathf.Lerp(maxMoonlightIntensity, 0, lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));

    }

}
