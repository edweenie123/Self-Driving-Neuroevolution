using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float timeScale = 1f;
    float minTime = 1f;
    float maxTime = 10f;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.RightArrow)) ChangeTime(1f);

        if (Input.GetKeyUp(KeyCode.LeftArrow)) ChangeTime(-1f);

        if (Input.GetKeyUp(KeyCode.R)) SystemSettings.visualizeRayToggle = !SystemSettings.visualizeRayToggle;
     
    }

    void ChangeTime(float sign)
    {
        // update the timeScale value and ensure it's within the range [minTime, maxTime]
        timeScale += sign;
        timeScale = Mathf.Min(maxTime, timeScale);
        timeScale = Mathf.Max(minTime, timeScale);
    
        Time.timeScale = timeScale;
        // update the UI to show the correct time scale
        UIManager.EditText(UIManager.timeScaleText, "Time Scale: " + timeScale.ToString("F1") + "x");
    }
}
