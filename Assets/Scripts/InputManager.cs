using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float timeScale = 1f;
    float changeTimeRate = 0.5f;
    float minTime = 1f;
    float maxTime = 10f;

    void Update() 
    {
        if (Input.GetKey(KeyCode.RightArrow)) {
            ChangeTime(1f);
        } 
        
        if (Input.GetKey(KeyCode.LeftArrow)) {
            ChangeTime(-1f);
        }
    }

    void ChangeTime(float sign) {
        // update the timeScale value and ensure it's within the range [minTime, maxTime]
        timeScale += sign * changeTimeRate * Time.deltaTime;
        timeScale = Mathf.Min(maxTime, timeScale);
        timeScale = Mathf.Max(minTime, timeScale);

        Time.timeScale = timeScale;
        // update the UI to show the correct time scale
        UIManager.EditText(UIManager.timeScaleText, "Time Scale: " + timeScale.ToString("F1") + "x");
    }
}
