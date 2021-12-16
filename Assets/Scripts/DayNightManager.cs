using System;
using UnityEngine;
using UnityEngine.Rendering;

public class DayNightManager : MonoBehaviour
{
    [SerializeField, Tooltip("Cannot be changed at runtime")]
    float realTimeMinutes;

    float rotateAmount;

    [SerializeField, Tooltip("Initial time of day offset")]
    float startingTime;

    float initialRotation;

    [SerializeField]
    GameObject sun, moon;

    public bool isDay = true;

    private void Start()
    {
        // Calculate rotation value
        initialRotation = startingTime * 60.0f;

        rotateAmount = 360.0f / (realTimeMinutes * 60.0f);

        // rotate the sun to initial position
        //         sun.transform.Rotate(initialRotation * rotateAmount, 0, 0, Space.Self);
        //         moon.transform.Rotate(initialRotation * rotateAmount, 0, 0, Space.Self);
        transform.Rotate(initialRotation * rotateAmount, 0, 0, Space.Self);
    }

    private void Update()
    {
        // Check if it is day or night
        if (transform.localEulerAngles.x > 0 && transform.localEulerAngles.x < 215 || transform.localEulerAngles.x > 360)
        {
            isDay = true;
        }
        else
        {
            isDay = false;
        }

        // Possible solution is using animation curves for this
        //TODO make light intensity slowly turn on instead of just turning on light game object
        if(isDay)
        {
            sun.GetComponent<Light>().shadows = LightShadows.Soft;
            moon.GetComponent<Light>().shadows = LightShadows.None;
        }
        else
        {
            sun.GetComponent<Light>().shadows = LightShadows.None;
            moon.GetComponent<Light>().shadows = LightShadows.Soft;
        }

        transform.Rotate(rotateAmount * Time.deltaTime, 0, 0, Space.Self);

        ChangeLights();
    }

    void ChangeLights()
    {
        if (isDay)
            RenderSettings.sun = sun.GetComponent<Light>();
        else
            RenderSettings.sun = moon.GetComponent<Light>();
    }
}
