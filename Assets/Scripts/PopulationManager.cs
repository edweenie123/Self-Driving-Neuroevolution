using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;
using System.Linq;


public class PopulationManager : MonoBehaviour
{
    public GameObject carPrefab;
    public GameObject carHolder;
    public Vector3 startPosition;

    // public GameObject bestNNHolder;

    int populationSize = 50;
    float populationTime = 100f;
    float checkAllDeadInterval = 0.5f;
    float timer = 0;
    float timer2 = 0;

    int generationNumber = 1;

    List<NeuralNetwork> lastGeneration = new List<NeuralNetwork>();
    List<NeuralNetwork> currentGeneration = new List<NeuralNetwork>();
    // List<List<GameObject>> generations = new List<List<GameObject>>();
    public List<float> matingPool;

    public List<float> averageFitnesses = new List<float>();
    float matingPoolSum; // sum of all fitnesses in mating pool

    public static float mutationRate = 0.05f;
    // public static float mutationMagnitude = 0.15f;

    void Start()
    {
        matingPool = new List<float>();
        lastGeneration = new List<NeuralNetwork>();
        currentGeneration = new List<NeuralNetwork>();

        CreateStartingGeneration();
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

    void CreateStartingGeneration()
    {
        currentGeneration.Clear();

        for (int i = 0; i < populationSize; i++)
        {
            // instantiate a car and set it's neural network weights to be equal to the child's
            GameObject t = Instantiate(carPrefab, startPosition, Quaternion.identity);
            NeuralNetwork childNetwork = t.GetComponent<NeuralNetwork>();

            childNetwork.InitializeNetwork();
            childNetwork.RandomizeWeights();

            currentGeneration.Add(childNetwork);
        }

        // update the generation text ui
        UIManager.EditText(UIManager.generationText, "Generation: " + generationNumber);
        generationNumber++;
    }

    void CreateNewGeneration()
    {
        timer2 = 0;

        // add all elements from the current generation array to the last generation array
        lastGeneration.Clear();
        foreach (var c in currentGeneration) lastGeneration.Add(c);


        
        CreateMatingPool();

        // print("printnig mating pool");
        // foreach (var m in matingPool) {
        //     print(m);
        // }
        
        // THIS DOESN'T WORK AND IDK WHY
        NeuralNetwork bestNN = GetBestMemberInPopulation();

        // print("bestNN weights");
        // print(bestNN.weights[0]);
        // print(bestNN.weights[1]);
        
        
        // destroy everything in the last generation
        foreach (var c in currentGeneration) Destroy(c.gameObject);
        currentGeneration.Clear();


        // generate the new generation
        for (int i = 0; i < populationSize; i++)
        {
            if (i==0) {
                GameObject bestT = Instantiate(carPrefab, startPosition, Quaternion.identity);
                NeuralNetwork bestTNetwork = bestT.GetComponent<NeuralNetwork>();
                bestTNetwork.InitializeNetwork();
                bestTNetwork.SetWeights(bestNN);
                bestT.GetComponent<CarMovement>().isTheBoi = 100;
                currentGeneration.Add(bestTNetwork);
            } else {
                // find two parents (probability based off fitness) and make a child using crossover
                NeuralNetwork parentA = SelectParent();
                NeuralNetwork parentB = SelectParent();

                // instantiate a car and set it's neural network weights to be equal to the crossover of A and B
                GameObject t = Instantiate(carPrefab, startPosition, Quaternion.identity);
                // t.transform.parent = carHolder.transform; 
                NeuralNetwork childNetwork = t.GetComponent<NeuralNetwork>();

                childNetwork.InitializeNetwork();
                childNetwork.Crossover(parentA, parentB);
                childNetwork.Mutate(); // mutate the child a little
                // childNetwork.RandomizeWeights();
                currentGeneration.Add(childNetwork);

            }

        }



        // update the generation text ui
        UIManager.EditText(UIManager.generationText, "Generation: " + generationNumber);
        generationNumber++;
    }

    void CreateMatingPool()
    {
        matingPool.Clear();
        matingPoolSum = 0;

        // add every fitness to the mating pool and update the mating pool sum
        foreach (var c in lastGeneration)
        {
            float carFitness = c.gameObject.GetComponent<CarMovement>().fitness;
            matingPool.Add(carFitness);
            matingPoolSum += carFitness;
        }
    }

    NeuralNetwork GetBestMemberInPopulation()
    {
        float bestFitness = 0f;
        int bestIdx = 0;

        for (int i = 0; i < populationSize; i++)
        {
            if (matingPool[i] > bestFitness) {
                bestFitness = matingPool[i];
                bestIdx = i;
            }
        }
       
        averageFitnesses.Add(bestFitness);

        // print("best boi here is " + bestIdx + " with fitness of " + bestFitness);

        return lastGeneration[bestIdx];
    }

    // selects a parent based off the mating pool
    NeuralNetwork SelectParent()
    {
        float randVal = Random.Range(0f, matingPoolSum); // determines the id of the parent
        float cumulativeSum = 0;

        // determine which interval randVal falls in within the mating pool
        for (int i = 0; i < matingPool.Count; i++)
        {
            cumulativeSum += matingPool[i];
            if (cumulativeSum >= randVal)
                return lastGeneration[i].GetComponent<NeuralNetwork>();
        }

        // print("big problem boi");
        // we will never reach this point (probably)
        return null;
    }
}
