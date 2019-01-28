using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTransitionManagerScript : MonoBehaviour {

    [SerializeField] private int index = 0;

    public Text storyTextBox;

    public List<string> text;

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
	void Update () {
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
    }

    public void Next()
    {
        if (index < maxIndex && !inTransition)
        {
            index++;
            inTransition = true;
        }
    }

    public void Previous()
    {
        if (index > 0 && !inTransition)
        {
            index--;
            inTransition = true;
        }
    }
}
