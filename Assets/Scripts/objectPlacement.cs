using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPlacement : MonoBehaviour
{
    public GameObject placePrefab;
    public KeyCode objectHotkey = KeyCode.A;
    public KeyCode increaseLenHotkey = KeyCode.D;
    public KeyCode DecreaseLenHotkey = KeyCode.S;
    public KeyCode deleteWallHotkey = KeyCode.Delete;

    private GameObject currentPlaceObject;
    private float rotation;

    public float rotateSpeed = 10f;
    public float changeLenAmount = 1f;


    GameObject selectedWall = null;

    // Update is called once per frame
    void Update()
    {
        HandleHotkey();

        if (currentPlaceObject != null)
        {
            MoveObject();
            MouseWheel();
            ReleaseClick();
            ChangeWallLen();
        }
        else
        {
            HandleSelection();
        }

    }

    void ChangeWallLen()
    {
        if (Input.GetKeyDown(increaseLenHotkey))
            currentPlaceObject.transform.localScale += new Vector3(changeLenAmount, 0, 0);
        else if (Input.GetKeyDown(DecreaseLenHotkey))
            if (currentPlaceObject.transform.localScale.x > 2 * changeLenAmount)
                currentPlaceObject.transform.localScale += new Vector3(-changeLenAmount, 0, 0);
    }

    private void ReleaseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentPlaceObject = null;
        }
    }

    // rotate the object using mouse wheel
    private void MouseWheel()
    {
        rotation = Input.mouseScrollDelta.y;
        currentPlaceObject.transform.Rotate(Vector3.up, rotation * rotateSpeed);
    }

    private void MoveObject()
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

    // check if user clicks on any of the walls
    void HandleSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 1000f, 1 << LayerMask.NameToLayer("wallLayer")))
            {
                GameObject hitWall = hitInfo.transform.gameObject;

                // deselect previous wall if user clicks on new wall
                if (!GameObject.ReferenceEquals(selectedWall, hitWall))
                {
                    DeselectCurrentWall();
                    selectedWall = hitWall;
                    selectedWall.GetComponent<Animator>().SetBool("isSelected", true);
                }
            }
            else
            {
                DeselectCurrentWall();
            }
        }

        HandleDeleteWalls();
    }

    void DeselectCurrentWall()
    {
        if (selectedWall != null)
        {
            selectedWall.GetComponent<Animator>().SetBool("isSelected", false);
            selectedWall = null;
        }
    }

    void HandleDeleteWalls() 
    {
        if (Input.GetKeyDown(deleteWallHotkey) && selectedWall != null)
            Destroy(selectedWall);
    }


    private void HandleHotkey()
    {
        // create / destroy wall prefab
        if (Input.GetKeyDown(objectHotkey))
        {
            if (currentPlaceObject == null) currentPlaceObject = Instantiate(placePrefab);
            else Destroy(currentPlaceObject);
        }
    }
}
