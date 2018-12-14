using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handleMovement : MonoBehaviour
{
    public float speed = 4.0f;
    public float jumpSpeed = 4.0f;
    public float fallMultiplier = 2.5f; //these handle the flying thing
    public float lowJumpMultiplier = 2f; //these handle the flying thing
    public GameObject playerCamera;

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        HandleMovement();
	}

    public void HandleMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            Vector3 cameraForward = playerCamera.transform.forward;
            cameraForward.y = 0;
            transform.forward = cameraForward;
            moveDirection += cameraForward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            Vector3 cameraBackward = -playerCamera.transform.forward;
            cameraBackward.y = 0;
            transform.forward = cameraBackward;
            moveDirection += cameraBackward;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Vector3 cameraLeft = playerCamera.transform.forward;
            cameraLeft.y = 0;
            transform.right = cameraLeft;
            moveDirection += -playerCamera.transform.right;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Vector3 cameraRight = -playerCamera.transform.forward;
            cameraRight.y = 0;
            transform.right = cameraRight;
            moveDirection += playerCamera.transform.right;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.up * jumpSpeed;

            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 &&  !Input.GetKey(KeyCode.Space))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime; 
            }
        }

        transform.position += moveDirection * Time.deltaTime * speed;
    }
}
