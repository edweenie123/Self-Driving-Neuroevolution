using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public GameObject carPrefab;
    public Vector3 startPosition;

    int populationSize = 15;
    float populationTime = 25f;
    float checkAllDeadInterval = 0.5f;
    float timer = 0;
    float timer2 = 0;

    int generationNumber = 1;

    List<GameObject> currentGeneration = new List<GameObject>();

    void Start()
    {
        CreateNewGeneration();
    }

    void Update()
    {
        timer += Time.deltaTime;
        timer2 += Time.deltaTime;

        if (timer > checkAllDeadInterval)
        {
            if (EntireGenerationDead()) CreateNewGeneration();
            timer = 0;
        }

        if (timer2 > populationTime)
        {
            CreateNewGeneration();
            timer2 = 0;
        }
    }

    // returns true if all cars in current generation are dead
    bool EntireGenerationDead()
    {
        bool allDead = true;
        foreach (var car in currentGeneration)
        {
            if (!car.GetComponent<CarMovement>().isDead)
            {
                allDead = false;
                break;
            }
        }

        return allDead;
    }

    void CreateNewGeneration()
    {
        // update the generation text ui
        UIManager.EditText(UIManager.generationText, "Generation: " + generationNumber);
        generationNumber++;

        // destroy everything in the last generation
        foreach (var c in currentGeneration) Destroy(c);
        currentGeneration.Clear();

        // create the new generation and it to the currentGernation list
        for (int i = 0; i < populationSize; i++)
        {
            GameObject t = Instantiate(carPrefab, startPosition, Quaternion.identity);
            currentGeneration.Add(t);
        }
    }

    // takes two parents A and B => returns set's the weights of child
    NeuralNetwork Crossover(NeuralNetwork A, NeuralNetwork B, NeuralNetwork child)
    {
        child.InitializeNetwork();
        
        // equal chance of having either parent's biases
        for (int i = 0; i < child.biases.Count; i++) {
            for (int j = 0; j < child.biases[i].ColumnCount; j++) {
                if (Random.Range(0f, 1f) > 0.5f)    child.biases[i][0, j] = A.biases[i][0, j];
                else                                child.biases[i][0, j] = B.biases[i][0, j];     
            }
        }

        // equal chance of having either parent's weights 
        for (int i = 0; i < child.weights.Count; i++) {
            for (int j = 0; j < child.weights[i].RowCount; j++) {
                for (int k = 0; k < child.weights[i].ColumnCount; k++) {
                    child.weights[i][j, k] = Random.Range(-1f, 1f);
                    if (Random.Range(0f, 1f) > 0.5f)    child.weights[i][j, k] = A.weights[i][j, k];
                    else                                child.weights[i][j, k] = B.weights[i][j, k];   
                }
            }
        }

        return child;
    }
}
