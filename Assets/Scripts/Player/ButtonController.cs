using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{

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
        if (Input.GetButtonDown("Reload")) // Reload Player Position
        {
            ControllerScript.setCursorLocked(false);
            SceneManager.LoadScene("menu");
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
