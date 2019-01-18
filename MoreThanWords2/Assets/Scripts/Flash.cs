
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Flash : MonoBehaviour
{
    private int part;


    // Use this for initialization
	/*void Start ()
    {
	}*/


    /*// Update is called once per frame
	void Update ()
    {
	}*/


    // Causes Kallum or any enemy to flash if he's been hit during a certain amount of time that is received as a parameter. The function returns a float to update the 'flashCounter' parameter of the character.
    public float EnemyFlash (float flashCounter, float flashLength, GameObject sprites)
    {
        part = (int) (flashCounter / (0.2f * flashLength));

        if (part % 2 == 0)
        {
            sprites.SetActive (true);
        }
        else
        {
            sprites.SetActive (false);
        }

        flashCounter -= Time.deltaTime;

        return flashCounter;
    }


    public float PlayerFlash (float flashCounter, float flashLength, Transform[] sprites)
    {
        /*int part = (int) (flashCounter / (0.2f * flashLength));
        bool activation;

        if (part % 2 == 0)
        {
            activation = true;
        }
        else
        {
            activation = false;
        }

        foreach (Transform child in sprites)
        {
            child.gameObject.SetActive (activation);
        }*/

        flashCounter -= Time.deltaTime;

        return flashCounter;
    }
}