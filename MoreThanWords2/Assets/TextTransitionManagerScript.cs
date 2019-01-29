using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextTransitionManagerScript : MonoBehaviour {

    [SerializeField] private int index = 0;

    public Text storyTextBox;

    public List<string> text;

    public Button next;
    public Button prev;

    [SerializeField] private int maxIndex = 0;

    private bool inTransition = false;

    public float transition = 0.5f;

    public Canvas canvas;

    [SerializeField] private float transitionTimer = 0;

    // Use this for initialization
    private void Awake()
    {
        SetTextList(canvas.GetComponent<InformationScript>().GetText());
    }

    void Start () {
        //Testing();
        //text = canvas.GetComponent<InformationScript>().GetText();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (inTransition)
        {
            transitionTimer += Time.deltaTime;
            Color currentColor = storyTextBox.color;
            if (transitionTimer < transition / 2)
            {
                currentColor.a = 1 - (transitionTimer / (transition / 2));
            }
            else
            {
                storyTextBox.text = text[index];
                currentColor.a = (transitionTimer - transition / 2) / (transition / 2);
            }
            if (transitionTimer > transition)
            {
                currentColor.a = 1;
                inTransition = false;
                transitionTimer = 0;
            }
            storyTextBox.color = currentColor;
        }
        else
        {
            if (Input.GetKeyDown (KeyCode.RightArrow) == true || Input.GetAxis ("DPadX") > 0)
            {
                Next ();
            }

            if (Input.GetKeyDown (KeyCode.LeftArrow) == true || Input.GetAxis ("DPadX") < 0)
            {
                Previous ();
            }
        }
    }


    public void ResetValues()
    {
        index = 0;
        inTransition = false;
        maxIndex = 0;
    }

    public void Testing()
    {
        maxIndex = text.Count-1;
        storyTextBox.text = text[0];
    }

    public void SetTextList(List<string> i)
    {
        text = i;
        maxIndex = i.Count-1;
        storyTextBox.text = text[0];
        UpdateButtons();
    }

    public void Next()
    {
        if (index < maxIndex && !inTransition)
        {
            index++;
            inTransition = true;
        }
        UpdateButtons();
    }

    public void Previous()
    {
        if (index > 0 && !inTransition)
        {
            index--;
            inTransition = true;
        }
        UpdateButtons();
    }

    public void UpdateButtons()
    {
        if (index == maxIndex)
        {
            next.image.enabled = false;
            next.GetComponentInChildren<Text>().text = "";
        }
        else if (index == maxIndex -1)
        {
            next.image.enabled = true;
            next.GetComponentInChildren<Text>().text = "Next ->";
        }
        if (index == 0)
        {
            prev.image.enabled = false;
            prev.GetComponentInChildren<Text>().text = "";
        }
        else if (index == 1)
        {
            prev.image.enabled = true;
            prev.GetComponentInChildren<Text>().text = "<- Prev";
        }
    }
}
