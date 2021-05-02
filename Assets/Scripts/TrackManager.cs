using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public GameObject track1, track2, track3;

    void Start() 
    {
        // change the loaded track depending on the load id
        switch(GlobalVariables.loadTrackID)
        {
            case 1: track1.SetActive(true); break;
            case 2: track2.SetActive(true); break;
            case 3: track3.SetActive(true); break;
        }
    }
}
