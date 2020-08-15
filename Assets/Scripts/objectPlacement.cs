using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPlacement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject placePrefab;

    [SerializeField]
    private KeyCode objectHotkey = KeyCode.A;

    private GameObject currentPlaceObject;
    private float rotation;

    // Update is called once per frame
    void Update()
    {
        handleHotkey();

        if(currentPlaceObject != null) {
            moveObject();
            mouseWheel();
            releaseClick();
            heightChange();
            heightDecrease();
        }
    }

    private void heightChange() {
        int factor = 0;
        
        if(Input.GetKeyDown(KeyCode.S)) {
            factor +=1;
            currentPlaceObject.transform.localScale = new Vector3 (currentPlaceObject.transform.localScale.x + factor, currentPlaceObject.transform.localScale.y, currentPlaceObject.transform.localScale.z);
        }

        if(Input.GetKeyUp(KeyCode.S)) {
            factor += 1;
        }
    }

    private void heightDecrease() {
        int factor = 0;
        
        if(Input.GetKeyDown(KeyCode.D)) {
            if(currentPlaceObject.transform.localScale.x + factor == 0) {
                factor = 0;
            }
            else {
                factor -=1;
            }
            
            currentPlaceObject.transform.localScale = new Vector3 (currentPlaceObject.transform.localScale.x + factor, currentPlaceObject.transform.localScale.y, currentPlaceObject.transform.localScale.z);
        }

    }
    private void releaseClick() {
        if(Input.GetMouseButtonDown(0)) {
            currentPlaceObject = null;
        }
    }
    private void mouseWheel() {
        rotation = Input.mouseScrollDelta.y;
        currentPlaceObject.transform.Rotate(Vector3.up, rotation * 10f);
    }
    private void moveObject() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo)) {
            currentPlaceObject.transform.position = hitInfo.point;
        }

    }
    private void handleHotkey() {
        if(Input.GetKeyDown(objectHotkey)) {
            if(currentPlaceObject == null) {
                currentPlaceObject = Instantiate(placePrefab);
            }
            else {
                Destroy(currentPlaceObject);
            }
        }
    }
}
