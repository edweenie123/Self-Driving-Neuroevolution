using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public Vector3 startPosition, startRotation;


    [Header("Car Settings")]
    public float carSpeed;
    public float turnSpeed;
    // public float sensorDist;


    [Header("Fitness Settings")]
    public float distanceWeight;
    public float speedWeight;

    [Range(-1f, 1f)]
    public float a, t;

    [Header("Statistics")]
    public float timer = 0;
    public float totalDistance = 0;
    public float avgSpeed;
    public float fitness;


    private float sensorA, sensorB, sensorC;
    Vector3 lastPosition;

    private void STart()
    {
        startPosition = transform.position;
        startRotation = transform.eulerAngles;
    }

    public void Reset()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "wall")
        {
            print("you hit a wall boi");
        }
    }


    void UpdateFitness()
    {
        totalDistance += Vector3.Distance(transform.position, lastPosition);
        avgSpeed = totalDistance / timer;
        fitness = totalDistance * distanceWeight + avgSpeed * speedWeight;
    }


    void InputSensors()
    {
        Vector3 right = (transform.forward + transform.right);
        Vector3 forward = (transform.forward);
        Vector3 left = (transform.forward - transform.right);

        Ray r = new Ray(transform.position, right);

        RaycastHit hit;

        Debug.DrawRay(transform.position, forward*10, Color.red);

        if (Physics.Raycast(r, out hit))
        {
            sensorA = hit.distance;
            Debug.DrawRay(transform.position, hit.point - transform.position, Color.red);
            // print("right: " + sensorA);
        }

        r.direction = forward;

        if (Physics.Raycast(r, out hit, 100))
        {
            sensorB = hit.distance;
            Debug.DrawRay(transform.position, hit.point - transform.position, Color.red);
        }

        r.direction = left;

        if (Physics.Raycast(r, out hit))
        {
            sensorC = hit.distance;
            Debug.DrawRay(transform.position, hit.point - transform.position, Color.red);

            // print("left: " + sensorC);
        }
    }

    public void MoveCar(float accel, float rot)
    {
        Vector3 inp = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, accel * carSpeed * Time.deltaTime), 0.02f);
        inp = transform.TransformDirection(inp);
        transform.position += inp;
        transform.eulerAngles += new Vector3(0, rot * turnSpeed * Time.deltaTime, 0);
    }

    private void FixedUpdate()
    {
        InputSensors();
        lastPosition = transform.position;

        MoveCar(a, t);

        timer += Time.deltaTime;

        UpdateFitness();
    }

}
