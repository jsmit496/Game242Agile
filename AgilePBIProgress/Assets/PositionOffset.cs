using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionOffset : MonoBehaviour
{
    public Transform objectToFollow;
    public Vector3 offset;

    Vector3 position;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        position = objectToFollow.position + offset;
        transform.position = position;
	}
}
