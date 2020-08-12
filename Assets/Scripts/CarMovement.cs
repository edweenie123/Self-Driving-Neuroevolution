using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public Vector3 startPosition, startRotation;
    Rigidbody rb;

    [Header("Car Settings")]
    public float carSpeed;
    public float turnSpeed;

    [Header("Statistics")]
    public float timer = 0;
    public float totalDistance = 0;
    public float avgSpeed;
    public float fitness;

    [Header("Network Architecture")]
    public int hiddenLayerCnt = 1;
    public int nodeCount = 10;
    NeuralNetwork network;

    private List<float> sensorDistances = new List<float>();
    
    
    float fovAngle = 130f;
    int numSensors = 4;
    float sensorUpdateTime = 0.2f;

    Vector3 lastPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.eulerAngles;
        network = GetComponent<NeuralNetwork>();

        network.InitializeNetwork(numSensors, hiddenLayerCnt, nodeCount);
    }

    // private void OnCollisionEnter(Collision other)
    // {
    //     if (other.gameObject.tag == "wall")
    //     {
    //         print("you hit a wall boi");
    //     }
    // }

    void UpdateSensors()
    {   
        // get all the directional vectors for the sensors based off the fiew of view and number of sensors
        Vector3 dir = Quaternion.Euler(0, -fovAngle / 2f, 0) * transform.forward;
        List<Vector3> sensorVectors = new List<Vector3>();

        for (int i = 0; i < numSensors; i++)
        {
            sensorVectors.Add(dir);
            dir = Quaternion.Euler(0, fovAngle / ((float)numSensors - 1f), 0) * dir;
        }

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
                Debug.DrawRay(transform.position, hit.point - transform.position, Color.red);
            }

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
        
        if (timer > sensorUpdateTime) {
            UpdateSensors();
            (accel, rotationAngle) = network.ForwardPropagate(sensorDistances);

            timer = 0;
        }

        MoveCar(accel, rotationAngle);
    }

}
