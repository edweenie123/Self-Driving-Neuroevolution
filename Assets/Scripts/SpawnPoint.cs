﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    bool selected = false;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GlobalVariables.isPausedEvolution)
        {
            HandleSelection();
            HandleMovement();
        }
    }

    void HandleSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 1000f, 1 << LayerMask.NameToLayer("spawnPointLayer")))
            {
                // release
                if (selected) Deselect();

                // select initially
                else Select();
            }
        }
    }

    void Select()
    {
        selected = true;
        animator.SetBool("isSelected", true);
    }

    void Deselect()
    {
        selected = false;
        animator.SetBool("isSelected", false);
    }


    void HandleMovement()
    {
        if (selected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 1000f, 1 << LayerMask.NameToLayer("planeLayer")))
            {
                // move the spawn point to the point where the curser hits the plane
                Vector3 newLoc = hitInfo.point;
                newLoc += new Vector3(0, transform.localScale.y / 2f, 0); // apply offset
                transform.position = newLoc;
            }
        }
    }

    public void SwitchVisibility() 
    {
        GetComponent<MeshRenderer>().enabled = !GetComponent<MeshRenderer>().enabled;
    }
}
