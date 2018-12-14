using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleCamera : MonoBehaviour
{
    public GameObject playerCamera;
    public Transform minimumHeightPoint;
    public Transform maximumHeightPoint;

    public Vector3 defaultCameraPosition;
    public Quaternion defaultCameraRotation;
    private bool canSeePlayer;
    private GameObject hitObject = null;
    private Vector3 referRotation;
    private Vector3 referPosition;
    public float distanceBetweenCameraPlayer;

    public float fadeSpeed = 0.05f;
    public float fadeAmount = 0.20f;

    // Use this for initialization
    void Start ()
    {
        float differenceX = playerCamera.transform.position.x - transform.position.x;
        float differenceY = playerCamera.transform.position.y - transform.position.y;
        float differenceZ = playerCamera.transform.position.z - transform.position.z;
        defaultCameraPosition = new Vector3(differenceX, differenceY, differenceZ);
    }
	
	// Update is called once per frame
	void Update ()
    {
        CameraHandler();
        AdjustCameraOnHeight();
        CameraViewPlayer();
    }

    public void SetupCamera()
    {
        playerCamera.transform.position = transform.position + defaultCameraPosition;
    }

    public void CameraHandler()
    {
        playerCamera.transform.position = transform.position + defaultCameraPosition;
        referRotation = playerCamera.transform.rotation.eulerAngles;
        referPosition = playerCamera.transform.position;
        distanceBetweenCameraPlayer = Vector3.Distance(transform.position, playerCamera.transform.position);

        RaycastHit hitInfo;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitInfo, distanceBetweenCameraPlayer))
        {
            if (hitInfo.transform.gameObject.tag != "Player" && hitInfo.transform.gameObject.GetComponent<Renderer>() != null)
            {
                if (hitObject != null && hitInfo.transform.gameObject != hitObject)
                {
                    for (float i = fadeAmount; i < 1f; i += fadeSpeed * Time.deltaTime)
                    {
                        Color c = hitObject.GetComponent<Renderer>().material.color;
                        c.a = i;
                        hitObject.GetComponent<Renderer>().material.color = c;
                    }
                }
                hitObject = hitInfo.transform.gameObject;
                foreach (Material mat in hitObject.GetComponent<Renderer>().materials)
                {
                    mat.SetFloat("_Mode", 2);
                    mat.SetInt("_srcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    mat.SetInt("_ZWrite", 0);
                    mat.DisableKeyword("_ALPHATEST_ON");
                    mat.EnableKeyword("_ALPHABLEND_ON");
                    mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    mat.renderQueue = 3000;
                }
                canSeePlayer = false;
                for (float i = 1f; i > fadeAmount; i -= fadeSpeed * Time.deltaTime)
                {
                    Color c = hitObject.GetComponent<Renderer>().material.color;
                    c.a = i;
                    hitObject.GetComponent<Renderer>().material.color = c;
                }
            }
            else
            {
                canSeePlayer = true;
                if (hitObject != null)
                {
                    for (float i = fadeAmount; i < 1f; i += fadeSpeed * Time.deltaTime)
                    {
                        Color c = hitObject.GetComponent<Renderer>().material.color;
                        c.a = i;
                        hitObject.GetComponent<Renderer>().material.color = c;
                    }
                }
            }
        }
    }

    public void CameraViewPlayer()
    {
        float distToCamera = Vector3.Distance(playerCamera.transform.position, transform.position);
        Vector3 dirToCamera = playerCamera.transform.position - transform.position;

        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, dirToCamera, distToCamera);
        foreach (RaycastHit hit in hits)
        {
            //print(hit.transform.gameObject.name);
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.GetComponent<Material>() != null)
            {
                Color oldColor = hitObject.GetComponent<Material>().color;
                hitObject.GetComponent<Material>().color = new Color(oldColor.r, oldColor.g, oldColor.b, 0.5f);
            }
        }
    }

    public void AdjustCameraOnHeight()
    {
        float distanceBetweenMinMax = maximumHeightPoint.position.y - minimumHeightPoint.position.y;
        float currentDistanceBetween = maximumHeightPoint.position.y - transform.position.y;

        if (transform.position.y <= minimumHeightPoint.position.y)
        {
            referRotation.x = 45;
            referPosition.y = defaultCameraPosition.y;
        }
        else if (transform.position.y >= maximumHeightPoint.position.y)
        {
            referRotation.x = 0;
            referPosition.y = 0f;
        }
        else if (transform.position.y >= minimumHeightPoint.position.y && transform.position.y < maximumHeightPoint.position.y)
        {
            float difference = currentDistanceBetween / distanceBetweenMinMax;
            referRotation.x = 45 * difference;
            referPosition.y = defaultCameraPosition.y * difference;
        }

        referPosition.y += transform.position.y;
        playerCamera.transform.rotation = Quaternion.Euler(referRotation);
        playerCamera.transform.position = referPosition;
    }

    private void OnDrawGizmos()
    {
        if (canSeePlayer)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * distanceBetweenCameraPlayer);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * distanceBetweenCameraPlayer);
        }
    }
}
