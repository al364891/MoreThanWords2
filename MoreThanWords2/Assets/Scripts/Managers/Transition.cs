
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public static Transition instance;
    [SerializeField] private Animator animator;
    private string level;


   void Awake ()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy (gameObject);

            return;
        }

        DontDestroyOnLoad (gameObject);
    }


    // Update is called once per frame
    void Update ()
    {
        /*if (Input.GetMouseButtonDown(0) == true)
        {
            FadeToLevel ("ScoreScene");
        }*/
	}


    public void FadeToLevel (string tag)
    {
        level = tag;
        animator.SetTrigger ("FadeOut");
    }


    public void OnFadeComplete ()
    {
        SceneManager.LoadScene (level);
    }
}