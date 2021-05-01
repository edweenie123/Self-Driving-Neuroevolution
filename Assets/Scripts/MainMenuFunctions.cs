using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuFunctions : MonoBehaviour
{
    
    public GameObject currentScreen;

    public void ShowScreen (GameObject screen)
    {
        currentScreen.SetActive(false);
        screen.SetActive(true);
        currentScreen = screen;
    }
}
