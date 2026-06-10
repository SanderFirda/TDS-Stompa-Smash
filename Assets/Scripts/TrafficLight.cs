using System;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    [SerializeField] private Light redLight;
    [SerializeField] private Light orangeLight;
    [SerializeField] private Light greenLight;

    public enum LightState
    {
        red,
        orange,
        green
    }
    public LightState lightState;

    [ContextMenu("Red Light")]
    void SetRedLight()
    {
        lightState = LightState.red;
    }

    [ContextMenu("Orange Light")]
    void SetOrangeLight()
    {
        lightState = LightState.orange;
    }

    [ContextMenu("Green Light")]
    void SetGreenLight()
    {
        lightState = LightState.green;
    }

    void Update()
    {
        switch (lightState)
        {
            case LightState.red:
                redLight.enabled = true;
                orangeLight.enabled = false;
                greenLight.enabled = false;
                break;
            case LightState.orange:
                redLight.enabled = false;
                orangeLight.enabled = true;
                greenLight.enabled = false;
                break;
            case LightState.green:
                redLight.enabled = false;
                orangeLight.enabled = false;
                greenLight.enabled = true;
                break;
        }
    }
}
