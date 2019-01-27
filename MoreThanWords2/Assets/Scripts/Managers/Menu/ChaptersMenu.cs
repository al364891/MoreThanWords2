
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;



public class ChaptersMenu : MonoBehaviour
{
    private Button[] chapters;
    [SerializeField] MainMenu menu;
    private int available;
    private string levelToLoad;


    void Start ()
    {
        chapters = new Button[5];
        
        switch (GameController.gameController.CheckLastUnlocked ())
        {
            case ("Level0"):
                available = 1;
                break;
            case ("Level1"):
                available = 2;
                break;
            case ("Level2"):
                available = 3;
                break;
            case ("Level3"):
                available = 4;
                break;
            case ("Level4"):
                available = 5;
                break;
        }

        // QUITAR EN LA VERSION FINAL
        available = 5;

        for (int i = 0; i < chapters.Length; i += 1)
        {
            chapters[i] = this.transform.GetChild(i).GetComponent<Button> ();
            if (i >= available)
            {
                chapters[i].interactable = false;
                //print("Not accessible");
            }
        }

        /*if (GameController.gameController.CheckLastUnlocked () == 1)
        {
            chapter2 = GameObject.Find("Chapter2But").GetComponent<Button> ();
            chapter2.interactable = false;
        }*/
    }


    private void LoadLevel (int index)
    {
        GameController.gameController.UsedChapterSelection ();
        levelToLoad = "Level" + index.ToString ();
        menu.transition.FadeToLevel (levelToLoad);
    }
}
