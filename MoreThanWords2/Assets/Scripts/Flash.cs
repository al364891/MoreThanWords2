
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Flash : MonoBehaviour
{
    private Color normal, flash;
    private Renderer gameMesh;
    private float delay;
    private int timesToFlash;

    
    // Use this for initialization
	void Start ()
    {
        normal = Color.white;
        flash = Color.red;
        delay = 0.025f;
        timesToFlash = 3;
	}


    public void FlashNow ()
    {
        var renderer = gameMesh;

        for (int i = 1; i <= timesToFlash; i += 1)
        {
            renderer.material.color = flash;
            yield return new WaitForSeconds (delay);
            renderer.material.color = normal;
            yield return new WaitForSeconds (delay);
        }
    }
	

	// Update is called once per frame
	void Update ()
    {
		
	}
}*/