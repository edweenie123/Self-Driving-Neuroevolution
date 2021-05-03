using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static Text generationText;
    public static Text timeScaleText;
    public Button playPauseButton;
    Text ppText;

    public Color playCol;
    public Color pauseCol;

    void Awake()
    {
        generationText = GameObject.Find("GenerationText").GetComponent<Text>();
        timeScaleText = GameObject.Find("TimeText").GetComponent<Text>();
        ppText = playPauseButton.GetComponentInChildren<Text>();
    }

    public static void EditText(Text textObject, string newText)
    {
        textObject.text = newText;
    }

    // gets called when play pause button is pressed
    public void PlayPause()
    {
        if (GlobalVariables.isPausedEvolution) 
        {
            EditText(ppText, "Pause");    
            playPauseButton.GetComponent<Image>().color = pauseCol;
        } 
        else 
        {
            EditText(ppText, "Start");
            playPauseButton.GetComponent<Image>().color = playCol;
        }

        GlobalVariables.isPausedEvolution = !GlobalVariables.isPausedEvolution;
    }
}
