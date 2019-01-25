
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;



public class GetChildren : MonoBehaviour
{
    public GameObject mainMenu, optionsMainMenu, chaptersMainMenu, instructionsMenu;
    public Button load, chapters;


    // Use this for initialization
    void Awake ()
    {
        mainMenu.SetActive (true);
        optionsMainMenu.SetActive (false);
        chaptersMainMenu.SetActive (false);
        instructionsMenu.SetActive (false);
    }


    /*// Update is called once per frame
    void Update ()
    {
		
	}*/
}