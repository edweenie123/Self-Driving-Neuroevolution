using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static Text generationText;
    // public static Text timeScaleText;
    public Button playPauseButton;
    Text ppText;

    public OptionButtonUI timeScaleButton;

    public SpawnPoint sp;

    public Color playCol;
    public Color pauseCol;

    public GameObject playingUI;
    public GameObject pauseUI;

    public float timeScale = 1f;

    void Awake()
    {
        // timeScaleText = GameObject.Find("TimeText").GetComponent<Text>();
        ppText = playPauseButton.GetComponentInChildren<Text>();
    }

    public static void EditText(Text textObject, string newText)
    {
        textObject.text = newText;
    }

    // gets called when play pause button is pressed
    public void PlayPause()
    {
        sp.SwitchVisibility();
        
        if (GlobalVariables.isPausedEvolution) 
        {
            EditText(ppText, "Pause");    
            playPauseButton.GetComponent<Image>().color = pauseCol;
            playingUI.SetActive(true);
            pauseUI.SetActive(false);

            generationText = GameObject.Find("GenerationText").GetComponent<Text>();
        } 
        else 
        {
            EditText(ppText, "Start");
            playPauseButton.GetComponent<Image>().color = playCol;
            
            playingUI.SetActive(false);
            pauseUI.SetActive(true);
        }

        GlobalVariables.isPausedEvolution = !GlobalVariables.isPausedEvolution;
        timeScaleButton.ResetTimeScale();
    }
}
