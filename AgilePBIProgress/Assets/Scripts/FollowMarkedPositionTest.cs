using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowMarkedPositionTest : MonoBehaviour
{
    public Transform staminaPosition;
    public GameObject stamina;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        stamina.transform.position = staminaPosition.position;
        stamina.transform.localEulerAngles = staminaPosition.localEulerAngles;
	}

    private void OnCollisionEnter(Collision collision)
    {
        staminaPosition = collision.gameObject.transform;
    }
}
