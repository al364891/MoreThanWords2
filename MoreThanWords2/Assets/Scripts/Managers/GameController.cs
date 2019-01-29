using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;



public class GameController : MonoBehaviour
{
    public static GameController gameController;
    private GetChildren canvas;
    [SerializeField] private GameObject mainMenu, optionsMainMenu, chaptersMainMenu, instructionsMenu;
    private Button load, chapters;
    private string saveLocation;
    //private string settingsLocation;
    [HideInInspector] public bool save;
    public bool findObjects;
    public Color gray;
    //[HideInInspector] public bool noSettings;

    /*public float masterAudio;
    public float effectsAudio;
    public float musicAudio;
    public bool fullscreen;
    public int graphics;
    public int resolutionX;
    public int resolutionY;*/


    void Awake ()
    {
        saveLocation = Application.persistentDataPath + "/Level.dat";
        //settingsLocation = Application.persistentDataPath + "Settings.dat";
        save = true;
        gray = new Vector4 (0.5f, 0.5f, 0.5f, 1);
        FindObjects ();

        if (gameController == null)
        {
            DontDestroyOnLoad(gameObject);
            gameController = this;
        }
        else if (gameController != this)
        {
            Destroy(gameObject);
        }

        if (File.Exists (saveLocation) == false)
        {
            load.interactable = false;
            load.transform.GetChild(0).GetComponent<Text>().color = gray;
            chapters.interactable = false;
            chapters.transform.GetChild(0).GetComponent<Text>().color = gray;
            //noSettings = true;
        }
        /*else
        {
            noSettings = false;
            LoadSettings();
        }*/

        /*if (File.Exists(settingsLocation) == false)
        {
            noSettings = true;
        }*/
    }


    void Update()
    {
        // COMENTAR AL FINAL
        /*if (Input.GetKeyDown (KeyCode.X) == true)
        {
            Delete ();
        }*/

        if (findObjects == true && SceneManager.GetActiveScene().name == "Menu")
        {
            FindObjects ();
        }
    }


    private void FindObjects ()
    {
        canvas = GameObject.Find("Canvas").GetComponent<GetChildren> ();
        mainMenu = canvas.mainMenu;
        chaptersMainMenu = canvas.chaptersMainMenu;
        optionsMainMenu = canvas.optionsMainMenu;
        instructionsMenu = canvas.instructionsMenu;
        load = canvas.load;
        chapters = canvas.chapters;

        findObjects = false;
        //print (findObjects);
    }


    public void Save()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter ();
        FileStream file = File.Create (saveLocation);

        /*data.masterAudio = masterAudio;
        data.effectsAudio = masterAudio;
        data.musicAudio = masterAudio;
        data.fullscreen = fullscreen;
        data.graphics = graphics;
        data.resolutionX = resolutionX;
        data.resolutionY = resolutionY;*/

        binaryFormatter.Serialize(file, SceneManager.GetActiveScene().name);
        file.Close();
    }


    /*public void SaveAudio(float volume)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(saveLocation);

        PlayerData data = new PlayerData();

        data.masterAudio = volume;
    }*/

    /*public void SaveSettings(float volume, bool fullscreen, int graphics, int resolution)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(saveLocation);

        PlayerData data = new PlayerData();

        data.masterAudio = volume;
        data.fullscreen = fullscreen;
        data.graphics = graphics;
        data.resolution = resolution;
    }*/


    public void Load()
    {
        if (File.Exists(saveLocation) == true)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter ();
            FileStream file = File.Open(saveLocation, FileMode.Open);

            save = true;

            mainMenu.GetComponent<MainMenu>().transition.FadeToLevel ((string) binaryFormatter.Deserialize (file));
            //SceneManager.LoadScene((string) binaryFormatter.Deserialize(file));
            file.Close();
            /*level = data.level;
            masterAudio = data.masterAudio;
            effectsAudio = data.effectsAudio;
            musicAudio = data.musicAudio;
            fullscreen = data.fullscreen;
            graphics = data.graphics;
            resolutionX = data.resolutionX;
            resolutionY = data.resolutionY;*/
        }
    }


    /*private void LoadSettings()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Open(saveLocation, FileMode.Open);

        PlayerData data = (PlayerData)binaryFormatter.Deserialize(file);
        file.Close();

        data.masterAudio =;
        Screen.fullScreen = data.fullscreen;
        QualitySettings.SetQualityLevel (data.graphics);
    }*/


    public void Delete()
    {
        if (SceneManager.GetActiveScene().name == "Menu" && File.Exists (saveLocation) == true)
        {
            File.Delete(saveLocation);
            load.interactable = false;
            load.transform.GetChild(0).GetComponent<Text>().color = gray;
            chapters.interactable = false;
            chapters.transform.GetChild(0).GetComponent<Text>().color = gray;

            print ("Deleted");
        }
    }


    public string CheckLastUnlocked()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Open (saveLocation, FileMode.Open);

        string last = (string) binaryFormatter.Deserialize(file);
        file.Close();

        return last;
    }


    public void UsedChapterSelection ()
    {
        save = false;
    }


    public void NormalPlay()
    {
        save = true;
    }
}
