using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public GameObject track1, track2, track3;
    public Transform spawnPoint;

    void Start() 
    {
        // change the loaded track depending on the load id
        switch(GlobalVariables.loadTrackID)
        {
            case 1: 
                track1.SetActive(true); 
                spawnPoint.position = new Vector3(-15.38f, 0.325f, -1.23f);
                break;
            case 2: 
                track2.SetActive(true); 
                spawnPoint.position = new Vector3(-17.1f, 0.325f, 0.65f);
                break;
            case 3: 
                track3.SetActive(true); 
                spawnPoint.position = new Vector3(-9.41f, 0.325f, -3.02f);
                break;
        }
    }
}
