using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuFunctions : MonoBehaviour
{

    public GameObject currentScreen;

    public void ShowScreen(GameObject screen)
    {
        currentScreen.SetActive(false);
        screen.SetActive(true);
        currentScreen = screen;
    }

    // change scene and change the track that gets loaded
    public void LoadTrack(int id)
    {
        GlobalVariables.loadTrackID = id;
        // print(GlobalVariables.loadTrackID);
        SceneManager.LoadScene("RealTrack");
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
