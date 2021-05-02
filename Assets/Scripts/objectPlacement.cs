using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPlacement : MonoBehaviour
{
    public GameObject placePrefab;   
    public KeyCode objectHotkey = KeyCode.A;
    public KeyCode increaseLenHotkey = KeyCode.D;
    public KeyCode DecreaseLenHotkey = KeyCode.S;

    private GameObject currentPlaceObject;
    private float rotation;

    public float rotateSpeed = 10f;
    public float changeLenAmount = 1f;

    // Update is called once per frame
    void Update()
    {
        handleHotkey();

        if (currentPlaceObject != null)
        {
            moveObject();
            mouseWheel();
            releaseClick();
            changeWallLen();
        }
    }

    void changeWallLen() 
    {
        if (Input.GetKeyDown(increaseLenHotkey)) 
            currentPlaceObject.transform.localScale += new Vector3(changeLenAmount, 0, 0);
        else if (Input.GetKeyDown(DecreaseLenHotkey))
            if (currentPlaceObject.transform.localScale.x > 2 * changeLenAmount)
                currentPlaceObject.transform.localScale += new Vector3(-changeLenAmount, 0, 0);
    }

    private void releaseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentPlaceObject = null;
        }
    }
    
    // rotate the object using mouse wheel
    private void mouseWheel()
    {
        rotation = Input.mouseScrollDelta.y;
        currentPlaceObject.transform.Rotate(Vector3.up, rotation * rotateSpeed);
    }

    private void moveObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 1000f, 1 << LayerMask.NameToLayer("planeLayer")))
        {
            Vector3 wallLocation = hitInfo.point;

            // adjust the wall position based off its height (so half the wall isn't beneath the plane)
            wallLocation += new Vector3(0, currentPlaceObject.transform.localScale.y / 2f, 0);

            currentPlaceObject.transform.position = wallLocation;
        }

    }

    private void handleHotkey()
    {
        // create / destroy wall prefab
        if (Input.GetKeyDown(objectHotkey))
        {
            if (currentPlaceObject == null) currentPlaceObject = Instantiate(placePrefab);
            else Destroy(currentPlaceObject);
        }
    }
}
