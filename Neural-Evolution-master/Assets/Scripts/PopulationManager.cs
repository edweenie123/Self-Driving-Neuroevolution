using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public GameObject carPrefab;
    public Vector3 startPosition;

    int populationSize = 15;
    float populationTime = 10f;
    public float timer = 0;
    
    int generationNumber = 1;

    List<GameObject> currentGeneration = new List<GameObject>();

    void Start() {
        CreateNewGeneration();
    }

    void Update() {
        timer += Time.deltaTime;

        if (timer > populationTime) {
            timer = 0;
            CreateNewGeneration();
        }
    }

    void CreateNewGeneration() {
        // update the generation text ui
        UIManager.EditText(UIManager.generationText, "Generation: " + generationNumber);
        generationNumber++;

        // destroy everything in the last generation
        foreach (var c in currentGeneration) Destroy(c);
        currentGeneration.Clear();

        // create the new generation and it to the currentGernation list
        for (int i=0;i<populationSize;i++) {
            GameObject t = Instantiate(carPrefab, startPosition, Quaternion.identity);
            currentGeneration.Add(t);
        }
    }
}   
