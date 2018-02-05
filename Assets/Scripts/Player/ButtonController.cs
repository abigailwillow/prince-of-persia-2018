using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{

    public Vector3 SpawnPosition = Vector3.zero;

    bool gamePaused = false;

    [HideInInspector]
    public bool debugMode = false;

    FPSController ControllerScript;
    void Awake()
    {
        ControllerScript = GetComponent<FPSController>();
    }

    void Update()
    {
        if (Input.GetButton("Reload")) // Reload Player Position
        {
            transform.position = SpawnPosition;
            ControllerScript.verticalVelocity = 0f;
        }

        if (Input.GetButtonDown("Menu")) // Menu Button Pressed
        {
            if (gamePaused)
            {
                ControllerScript.setCursorLocked(true);
                gamePaused = false;
            }
            else
            {
                ControllerScript.setCursorLocked(false);
                gamePaused = true;
            }
        }
    }
}
