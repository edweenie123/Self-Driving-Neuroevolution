using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static Text generationText;
    public static Text timeScaleText;

    void Awake() {
        generationText = GameObject.Find("GenerationText").GetComponent<Text>();    
        timeScaleText = GameObject.Find("TimeText").GetComponent<Text>();
    }

    public static void EditText(Text textObject, string newText)
    {
        textObject.text = newText;
    }
}
