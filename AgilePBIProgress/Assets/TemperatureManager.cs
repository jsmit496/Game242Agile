using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureManager : MonoBehaviour
{
    public float currentTemperature;
    public Transform temperatureManager;

    private float baseTemperature;

	// Use this for initialization
	void Start ()
    {
        SetBaseTemperature();
	}
	
	// Update is called once per frame
	void Update ()
    {
        SetBaseTemperature();
        SetTemperature();
	}

    public void SetTemperature()
    {
        if (transform.position.y > temperatureManager.position.y)
        {
            currentTemperature = baseTemperature - (transform.position.y - temperatureManager.position.y);
        }
        else
        {
            currentTemperature = baseTemperature;
        }
    }

    public void SetBaseTemperature()
    {
        baseTemperature = temperatureManager.GetComponent<TemperatureSetter>().baseTemperature;
    }
}
