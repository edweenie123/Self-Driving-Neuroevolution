using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public GameObject laserLinePrefab;

    Vector3 lastPosition;
    Vector3 startPosition, startRotation;
    float timer = 0;

    Rigidbody rb;
    NeuralNetwork network;

    [Header("Car Move Settings")]
    public float carSpeed;
    public float turnSpeed;

    [Header("Statistics")]
    public float totalDistance = 0;
    public float avgSpeed;
    public float fitness;

    List<float> sensorDistances = new List<float>();
    List<Vector3> sensorVectors = new List<Vector3>();
    List<LineRenderer> sensorLines = new List<LineRenderer>();
    float fovAngle = 130f;
    int numSensors = 4;

    float feedForwardInterval = 0.3f; // time between every forward propagation

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.eulerAngles;
        network = GetComponent<NeuralNetwork>();

        network.InitializeNetwork(numSensors);

        InitializeSensorLines();
    }

    void InitializeSensorLines()
    {
        for (int i = 0; i < numSensors; i++)
        {
            GameObject laserLine = Instantiate(laserLinePrefab, transform.position, Quaternion.identity);
            laserLine.transform.parent = gameObject.transform;
            sensorLines.Add(laserLine.GetComponent<LineRenderer>());
        }
    }

    // update sensor information and visualize the ray information
    void UpdateSensors()
    {
        GetSensorDirections();

        sensorDistances.Clear();
        RaycastHit hit;
        Ray r;

        // update the sensor ditances using the new raycast hit distances
        for (int i = 0; i < numSensors; i++)
        {
            r = new Ray(transform.position, sensorVectors[i]);

            if (Physics.Raycast(r, out hit))
            {
                sensorDistances.Add(hit.distance / 10f);

                if (SystemSettings.visualizeRayToggle)
                {
                    sensorLines[i].SetPosition(0, transform.position);
                    sensorLines[i].SetPosition(1, hit.point);
                }
                else
                {
                    sensorLines[i].SetPosition(0, Vector3.zero);
                    sensorLines[i].SetPosition(1, Vector3.zero);
                }
            }
            else
            {
                sensorLines[i].SetPosition(0, Vector3.zero);
                sensorLines[i].SetPosition(1, Vector3.zero);
            }
        }
    }

    // get all the directional vectors for the sensors based off the fiew of view and number of sensors
    void GetSensorDirections()
    {
        sensorVectors.Clear();
        Vector3 dir = Quaternion.Euler(0, -fovAngle / 2f, 0) * transform.forward;

        for (int i = 0; i < numSensors; i++)
        {
            sensorVectors.Add(dir);
            dir = Quaternion.Euler(0, fovAngle / ((float)numSensors - 1f), 0) * dir;
        }
    }

    public void MoveCar(float accel, float rot)
    {
        Vector3 inp = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, accel * carSpeed * Time.deltaTime), 0.02f);
        inp = transform.TransformDirection(inp);
        transform.position += inp;
        transform.eulerAngles += new Vector3(0, rot * turnSpeed * Time.deltaTime, 0);
    }

    float accel, rotationAngle;
    private void Update()
    {
        timer += Time.deltaTime;

        UpdateSensors();

        if (timer > feedForwardInterval)
        {
            (accel, rotationAngle) = network.ForwardPropagate(sensorDistances);
            timer = 0;
        }

        MoveCar(accel, rotationAngle);
    }

}
