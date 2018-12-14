using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalStats : MonoBehaviour
{
    public float maxHunger;
    public float maxThirst;
    public float minHunger; //This will be the point where it needs to eat
    public float minThirst; //This will be the point where it needs to drink
    public float decreaseRate;
    public float increaseAmount;

    public bool isHungry = false;
    public bool isThirsty = false;

    [SerializeField]
    float currentHunger, currentThirst;

	// Use this for initialization
	void Start ()
    {
        currentHunger = maxHunger;
        currentThirst = maxThirst;
	}
	
	// Update is called once per frame
	void Update ()
    {
        DetermineStatNeed();
        ChangeCurrentValues();
	}

    public void DetermineStatNeed()
    {
        if (currentHunger <= minHunger)
        {
            isHungry = true;
        }
        else if (currentHunger > minHunger)
        {
            isHungry = false;
        }

        if (currentThirst <= minThirst)
        {
            isThirsty = true;
        }
        else if (currentThirst > minThirst)
        {
            isThirsty = false;
        }

        if (currentThirst < 0)
        {
            currentThirst = 0;
        }
        else if (currentThirst > maxThirst)
        {
            currentThirst = maxThirst;
        }

        if (currentHunger < 0)
        {
            currentHunger = 0;
        }
        else if (currentHunger > maxHunger)
        {
            currentHunger = maxHunger;
        }
    }

    public void ChangeCurrentValues()
    {
        currentHunger -= decreaseRate * Time.deltaTime;
        currentThirst -= decreaseRate * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Food" && isHungry)
        {
            print("Found food");
            currentHunger += increaseAmount;
            Destroy(collision.collider.gameObject);
        }
    }

}
