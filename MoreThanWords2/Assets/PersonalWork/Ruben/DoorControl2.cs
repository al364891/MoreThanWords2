
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class DoorControl2 : MonoBehaviour
{
    public string siguienteEscena;
    private Animator animator;
    private GameObject player;
    private Transition transition;


    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator> ();
        player = GameObject.FindGameObjectWithTag ("Player");
        transition = GameObject.Find("Transition").GetComponent<Transition> ();
    }


    void OnTriggerEnter2D (Collider2D other)
    {
        bool doorOpen = animator.GetBool ("openDoor");

        if (other.gameObject == player)
        {
            if (doorOpen == true)
            {
                //solo cierra la puerta al tocar el jugador
                player.GetComponent<CharacterController2D>().finished = true;
                animator.SetBool ("openDoor", false);

                Invoke ("NextLevel", 2);
            }
        }
    }


    private void NextLevel ()
    {
        DontDestroyOnLoad (GameObject.FindGameObjectWithTag ("Manager"));
        GameObject.FindGameObjectWithTag("Manager").GetComponent<ScoreManagerScript>().NextScene = siguienteEscena;
        //SceneManager.LoadScene ("ScoreScene"); //Cambiar por el siguiente nivel cuando este
        transition.FadeToLevel ("ScoreScene");
		FindObjectOfType<AudioManager>().Stop("usingMagic");
    }
}