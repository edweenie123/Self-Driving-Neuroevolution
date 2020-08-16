using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Linq;

public class NeuralNetwork : MonoBehaviour
{
    public int inputSize = 3;
    int hiddenLayerCnt = 1;
    int hiddenNeuronCnt = 5;

    public Matrix<float> inputLayer;
    public List<Matrix<float>> hiddenLayers = new List<Matrix<float>>();
    public Matrix<float> outputLayer = Matrix<float>.Build.Dense(1, 2);
    public List<Matrix<float>> weights = new List<Matrix<float>>();
    public List<Matrix<float>> biases = new List<Matrix<float>>();

    public void InitializeNetwork()
    {
        hiddenLayers.Clear();
        outputLayer.Clear();
        weights.Clear();
        biases.Clear();

        // initalize input layer based off the input size (number of sensors)
        inputLayer = Matrix<float>.Build.Dense(1, inputSize);

        // initialize weights matrix from input layer to hidden layer
        if (hiddenLayerCnt > 0)
        {
            Matrix<float> inputToHidden = Matrix<float>.Build.Dense(inputSize, hiddenNeuronCnt);
            weights.Add(inputToHidden);
        }

        // initialize wieghts matrix from hidden layer to hidden layer
        for (int i = 0; i < hiddenLayerCnt; i++)
        {
            Matrix<float> f = Matrix<float>.Build.Dense(1, hiddenNeuronCnt);
            hiddenLayers.Add(f);

            // add weight matrix from current layer to next hidden layer
            if (i != hiddenLayerCnt - 1)
            {
                Matrix<float> hiddenToHidden = Matrix<float>.Build.Dense(hiddenNeuronCnt, hiddenNeuronCnt);
                weights.Add(hiddenToHidden);
            }
        }

        // initalize bias matrix for hidden layers
        for (int i = 0; i < hiddenLayerCnt; i++)
        {
            biases.Add(Matrix<float>.Build.Dense(1, hiddenNeuronCnt));
        }

        // add bias matrix for output layer
        biases.Add(Matrix<float>.Build.Dense(1, 2));

        // initalize weights matrix from the last hidden layer / input layer to output layer
        Matrix<float> outputMatrix;
        if (hiddenLayerCnt > 0) outputMatrix = Matrix<float>.Build.Dense(hiddenNeuronCnt, 2);
        else outputMatrix = Matrix<float>.Build.Dense(inputSize, 2);
        weights.Add(outputMatrix);
    }

    public void RandomizeWeights()
    {
        // randomize the value for every bias value
        for (int i = 0; i < biases.Count; i++)
            for (int j = 0; j < biases[i].ColumnCount; j++)
                biases[i][0, j] = Random.Range(-1f, 1f);

        // randomize the value for every weight
        for (int i = 0; i < weights.Count; i++)
            for (int j = 0; j < weights[i].RowCount; j++)
                for (int k = 0; k < weights[i].ColumnCount; k++)
                    weights[i][j, k] = Random.Range(-1f, 1f);
    }

    // sets neural network's weights to be identical to nn
    public void SetWeights(NeuralNetwork nn)
    {
        for (int i = 0; i < biases.Count; i++)
            for (int j = 0; j < biases[i].ColumnCount; j++)
                biases[i][0, j] = nn.biases[i][0, j];

        // randomize the value for every weight
        for (int i = 0; i < weights.Count; i++)
            for (int j = 0; j < weights[i].RowCount; j++)
                for (int k = 0; k < weights[i].ColumnCount; k++)
                    weights[i][j, k] = nn.weights[i][j, k];
    }

    public (float, float) ForwardPropagate(List<float> sensorInfo)
    {
        // feed the sensor information into the input layer and perform the activation function (tanh)
        for (int i = 0; i < sensorInfo.Count; i++) inputLayer[0, i] = sensorInfo[i];
        inputLayer = inputLayer.PointwiseTanh();

        // propogate the values through the hidden layers if they exist
        if (hiddenLayers.Count > 0)
        {
            hiddenLayers[0] = (inputLayer * weights[0] + biases[0]).PointwiseTanh();
            for (int i = 1; i < hiddenLayers.Count; i++)
                hiddenLayers[i] = (hiddenLayers[i - 1] * weights[i] + biases[i]).PointwiseTanh();
        }

        // propgate the values to the output layer from either the last hidden layer of the input layer (if there are no hidden layers)
        if (hiddenLayers.Count > 0) outputLayer = (hiddenLayers.Last() * weights.Last() + biases.Last()).PointwiseTanh();
        else outputLayer = (inputLayer * weights.Last() + biases.Last()).PointwiseTanh();

        // get accerlation and rotation values from the output layer
        float acceleration = sigmoid(outputLayer[0, 0]);
        float rotation = outputLayer[0, 1];

        return (acceleration, rotation);
    }

    float sigmoid(float k)
    {
        return 1f / (1 + Mathf.Exp(-k));
    }

    // takes two parents A and B => makes weights the crossover of A and B
    public void Crossover(NeuralNetwork A, NeuralNetwork B)
    {
        for (int i = 0; i < biases.Count; i++)
        {
            for (int j = 0; j < biases[i].ColumnCount; j++)
            {
                if (Random.Range(0f, 1f) > 0f) biases[i][0, j] = A.biases[i][0, j];
                else biases[i][0, j] = B.biases[i][0, j];
            }
        }

        // equal chance of having either parent's weights 
        for (int i = 0; i < weights.Count; i++)
        {
            for (int j = 0; j < weights[i].RowCount; j++)
            {
                for (int k = 0; k < weights[i].ColumnCount; k++)
                {
                    // weights[i][j, k] = Random.Range(-1f, 1f);
                    if (Random.Range(0f, 1f) > 0f) weights[i][j, k] = A.weights[i][j, k];
                    else weights[i][j, k] = B.weights[i][j, k];
                }
            }
        }
    }


    public void Mutate()
    {
        float mutationRate = PopulationManager.mutationRate;

        for (int i = 0; i < biases.Count; i++)
            for (int j = 0; j < biases[i].ColumnCount; j++)
                if (Random.Range(0f, 1f) < mutationRate)
                {
                    float rand = Random.Range(-1f, 1f);
                    biases[i][0, j] += rand;
                    biases[i][0, j] = Mathf.Clamp(biases[i][0, j], -1f, 1f);
                }

        // randomize the value for every weight
        for (int i = 0; i < weights.Count; i++)
            for (int j = 0; j < weights[i].RowCount; j++)
                for (int k = 0; k < weights[i].ColumnCount; k++)
                    if (Random.Range(0f, 1f) < mutationRate)
                    {
                        float rand = Random.Range(-1f, 1f);
                        weights[i][j, k] += rand;
                        weights[i][j, k] = Mathf.Clamp(weights[i][j, k], -1f, 1f);

                    }
    }


}