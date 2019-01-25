
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;



public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    [SerializeField] private GameObject pauseMenu, mainPauseMenu, optionsPauseMenu;
    private GameController gameController;
    private Transition transition;


    void Awake ()
    {
        transition = GameObject.Find("Transition").GetComponent<Transition>();
        gameController = GameObject.Find("GameController").GetComponent<GameController> ();

        pauseMenu.SetActive (false);
        mainPauseMenu.SetActive (false);
        optionsPauseMenu.SetActive (false);
    }


    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown (KeyCode.Escape) == true)
        {
            if (paused == true)
            {
                Resume ();
            }
            else
            {
                Pause ();
            }
        }
	}


    public void Resume ()
    {
        pauseMenu.SetActive (false);
        mainPauseMenu.SetActive (false);
        optionsPauseMenu.SetActive (false);
        Time.timeScale = 1f;
        paused = false;
    }


    private void Pause ()
    {
        pauseMenu.SetActive (true);
        mainPauseMenu.SetActive (true);
        optionsPauseMenu.SetActive (false);
        Time.timeScale = 0f;
        paused = true;
    }


    public void RestartLevel ()
    {
        Time.timeScale = 1f;
        transition.FadeToLevel (SceneManager.GetActiveScene().name);
    }


    public void LoadMenu ()
    {
        Time.timeScale = 1f;
        transition.FadeToLevel ("Menu");
        gameController.findObjects = true;
    }


    public void QuitGame ()
    {
        Debug.Log ("Quit");
        Application.Quit ();
    }
}
