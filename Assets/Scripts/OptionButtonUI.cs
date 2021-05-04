using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color normalColor = Color.red;
    public Color hoverColor = new Color(185f, 0f, 0f);
    
    Text childText;

    int timeScaleId = 0;
    float[] timeScaleOpts = {1f, 3f, 5f, 10f};


    void Start() {
        childText = GetComponentInChildren<Text>();
    }
    
    public void OnPointerEnter(PointerEventData eventData) 
    {
        childText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData) 
    {
        childText.color = normalColor;
    }

    public void ResetTimeScale() 
    {
        timeScaleId = 0;
        SystemSettings.timeScale = 1;
        if (childText != null) childText.text = "Time Scale: " + SystemSettings.timeScale + "x";
        Time.timeScale = SystemSettings.timeScale;
    }


    public void ChangeTimeScale() 
    {
        timeScaleId++; timeScaleId %= 4;
        SystemSettings.timeScale = timeScaleOpts[timeScaleId];
        childText.text = "Time Scale: " + SystemSettings.timeScale + "x";
        Time.timeScale = SystemSettings.timeScale;
    }

    public void ToggleRays() 
    {
        if (SystemSettings.visualizeRayToggle) childText.text = "Visualize Rays: OFF";
        else childText.text = "Visualize Rays: ON";
        SystemSettings.visualizeRayToggle = !SystemSettings.visualizeRayToggle;
    }

    public void ToggleParticleEffect() 
    {
        if (SystemSettings.particleEffectToggle) childText.text = "Particle Effects: OFF";
        else childText.text = "Particle Effects: ON";
        SystemSettings.particleEffectToggle = !SystemSettings.particleEffectToggle;
    }
}
