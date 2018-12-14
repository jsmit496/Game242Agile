using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFoV : MonoBehaviour
{
    public float maxRadiusDetectPlayer;
    public float maxAngleDetectPlayer = 30f;
    public float maxRadiusRandomPath;
    public float maxAngleRandomPath = 180f;
    public Grid grid;
    public GameObject target;
    public GameObject pathMarker;
    public LayerMask player;
    public LayerMask pathfindingMarker;
    public bool attackPlayer = false;
    public bool huntAnimals = false;

    AnimalStats animalStats;
    bool canSeePlayer = false;

	// Use this for initialization
	void Start ()
    {
        animalStats = gameObject.GetComponent<AnimalStats>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (attackPlayer && !animalStats.isHungry && !animalStats.isThirsty)
        {
            inFoV(target, maxAngleDetectPlayer, maxRadiusDetectPlayer);
        }
        if (huntAnimals)
        {
            if (animalStats.isHungry)
            {

            }
            else if (animalStats.isThirsty)
            {

            }
        }
        else if (!huntAnimals)
        {
            if (animalStats.isHungry)
            {
                FindFood();
            }
            else if (animalStats.isThirsty)
            {
                FindWater();
            }
        }
        if (!canSeePlayer && !animalStats.isHungry && !animalStats.isThirsty)
        {
            RandomMarker(maxAngleRandomPath, maxRadiusRandomPath);
        }
	}

    public void inFoV(GameObject target, float maxAngle, float maxRadius)
    {
        if (target != null && Physics.CheckSphere(transform.position, maxRadius, player))
        {
            Vector3 directionBetween = (target.transform.position - gameObject.transform.position).normalized;
            float angle = Vector3.Angle(gameObject.transform.forward, directionBetween);

            if (angle <= maxAngle)
            {
                Ray ray = new Ray(transform.position, target.transform.position - transform.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, maxRadius))
                {
                    if (hit.transform.gameObject == target)
                    {
                        print(string.Format("{0} is in the FOV", target.name));
                        canSeePlayer = true;
                        pathMarker.transform.position = target.transform.position;
                    }
                    else
                    {
                        canSeePlayer = false;
                    }
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
    }

    public void RandomMarker(float maxAngle, float maxRadius)
    {
        if (Physics.CheckSphere(transform.position, maxRadius, pathfindingMarker))
        {
            Vector2 positionV2 = new Vector2(transform.position.x, transform.position.y);
            Vector2 randomPoint = (Random.insideUnitCircle * (maxRadiusDetectPlayer * 2)) + positionV2;
            Vector3 newPosition = new Vector3(randomPoint.x, transform.position.y, randomPoint.y);
            Node n = grid.NodeFromWorldPosition(newPosition);
            if (n != null && n.walkable)
            {
                pathMarker.transform.position = n.position;
            }   
        }
    }

    public void FindFood()
    {
        GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");
        if (foods != null)
        {
            float closestDistance = Vector3.Distance(foods[0].transform.position, transform.position);
            GameObject closestFood = foods[0];
            foreach (GameObject food in foods)
            {
                float distance = Vector3.Distance(food.transform.position, transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestFood = food;
                }
            }
            Node n = grid.NodeFromWorldPosition(closestFood.transform.position);
            if (n!= null)
            {
                pathMarker.transform.position = n.position;
            }
        }
    }

    public void FindWater()
    {
        GameObject[] waters = GameObject.FindGameObjectsWithTag("Water");
        if (waters != null)
        {
            float closestDistance = Vector3.Distance(waters[0].transform.position, transform.position);
            GameObject closestFood = waters[0];
            foreach (GameObject water in waters)
            {
                float distance = Vector3.Distance(water.transform.position, transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestFood = water;
                }
            }
            Node n = grid.NodeFromWorldPosition(closestFood.transform.position);
            if (n != null)
            {
                pathMarker.transform.position = n.position;
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxRadiusDetectPlayer);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxRadiusRandomPath);

        Vector3 fovLine1 = Quaternion.AngleAxis(maxAngleDetectPlayer, transform.up) * transform.forward * maxRadiusDetectPlayer;
        Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngleDetectPlayer, transform.up) * transform.forward * maxRadiusDetectPlayer;

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        if (canSeePlayer)
        {
            Gizmos.color = Color.green;
        }
        else if (!canSeePlayer)
        {
            Gizmos.color = Color.black;
        }
        Gizmos.DrawLine(transform.position, target.transform.position);
    }
}
