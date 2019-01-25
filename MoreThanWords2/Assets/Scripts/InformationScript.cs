using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InformationScript : MonoBehaviour {
    GameObject manager;
    ScoreManagerScript scoreManager;
    private Transition transition;
    public Text storyText;
    private string nextScene;

    public Text totalScore;
    public Text killScore;
    public Text negativeScore;
    public Text timeScore;

    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        scoreManager = (ScoreManagerScript)FindObjectOfType(typeof(ScoreManagerScript));
        //Debug.Log(scoreManager);
        transition = GameObject.Find("Transition").GetComponent<Transition>();

        totalScore.text = "Total Score: " + scoreManager.getTotalScore();
        killScore.text = "Killing Points: " + scoreManager.getKillScore();
        negativeScore.text = "Fault Points: " + scoreManager.getNegativeScore();
        timeScore.text = "Time Bonus: " + scoreManager.getTimeScore();
        storyText.text = scoreManager.getStoryText();
        nextScene = scoreManager.getNextScene();
        

        Destroy(manager);
    }
	
    public void NextLevel()
    {
        transition.FadeToLevel (nextScene);
    }

}
