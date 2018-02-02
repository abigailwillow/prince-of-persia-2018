using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float runMultiplier = 1.5f;
    public float jumpStrength = 6f;
    public float mouseSensitivity = 2f;
    public float playerGravity = 20f;
    public Transform headBone;
    public Camera FirstPersonCamera;
    public Vector3 SpawnPosition = new Vector3(0, 0, 0);
    public LedgeGrabber hangScript;
    public TextMeshProUGUI debugGUI;

    float movementH;
    float movementV;
    float rotationX;
    float rotationY;
    float verticalVelocity;
    float curMultiplier = 0f;
    Vector3 curRotation = Vector3.zero;
    Vector3 deltaMovement = Vector3.zero;
    Animator anim;
    bool debugMode = false;
    bool gamePaused = false;
    Vector3 lastMove;

    [HideInInspector]
    public bool isGrounded = true;

    CharacterController ply;

    void Awake()
    {
        ply = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        setCursorLocked(true);
        headBone.transform.eulerAngles = curRotation;
    }

    public void setCursorLocked(bool boolean)
    {
        if (boolean)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) // Taking inputs
        {
            anim.SetFloat("MoveSpeed", 1f);
            curMultiplier = 1f;
        }
        else
        {
            anim.SetFloat("MoveSpeed", 0f);
            curMultiplier = 0f;
        }

        if (Input.GetAxisRaw("Vertical") > 0 && Input.GetButton("Sprint"))
        {
            anim.SetFloat("MoveSpeed", 1.5f);
            curMultiplier = runMultiplier;
        }
        movementH = Input.GetAxis("Horizontal");
        movementV = Input.GetAxis("Vertical");

        rotationX = Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        curRotation.x -= rotationY;
        curRotation.x = Mathf.Clamp(curRotation.x, -90, 90);
        curRotation.y = transform.eulerAngles.y;
        headBone.transform.eulerAngles = curRotation; // Mouse rotation
        FirstPersonCamera.transform.eulerAngles = curRotation;
        transform.Rotate(0, rotationX, 0); // Head rotation TO-DO SMOOTH

        deltaMovement = new Vector3(movementH, 0, movementV);
        deltaMovement = Vector3.Normalize(deltaMovement) * movementSpeed * curMultiplier;
        deltaMovement = transform.TransformDirection(deltaMovement);

        if (Input.GetButtonDown("Menu")) // Menu Button Pressed
        {
            if (gamePaused)
            {
                setCursorLocked(true);
                gamePaused = false;
            }
            else
            {
                setCursorLocked(false);
                gamePaused = true;
            }
        }

        if (Input.GetButtonDown("Debug")) // Debug Mode
        {
            if (debugMode)
            {
                debugMode = false;
                movementSpeed *= 0.5f;
                verticalVelocity = 0f;
                ply.detectCollisions = true;
            }
            else
            {
                debugMode = true;
                movementSpeed *= 2f;
                verticalVelocity = 0f;
                ply.detectCollisions = false;
            }
        }

        if (debugMode)
        {
            if (Input.GetButton("Jump"))
            {
                verticalVelocity = jumpStrength;
                deltaMovement.y = verticalVelocity;
            }

            if (Input.GetButton("Crouch"))
            {
                verticalVelocity = -jumpStrength;
                deltaMovement.y = verticalVelocity;
            }
        }
        else
        {
            if (hangScript.Hanging) // Hanging/climbing
            {
                isGrounded = hangScript.Hanging;
            }

            if (ply.isGrounded != isGrounded && !hangScript.Hanging)
            {
                isGrounded = ply.isGrounded;
            }

            if (isGrounded) // Jumping and gravity
            {
                if (Input.GetButtonDown("Jump"))
                {
                    verticalVelocity = jumpStrength;
                    deltaMovement = new Vector3(0, jumpStrength, 0) + lastMove;
                    ply.Move(deltaMovement);
                }
            }
            else
            {
                verticalVelocity -= playerGravity * Time.deltaTime;
            }
            deltaMovement.y = verticalVelocity;
            if (hangScript.Hanging)
            {
                deltaMovement.y = Mathf.Max(deltaMovement.y, 0);
            }
        }

        //deltaMovement.x = Mathf.Lerp(deltaMovement.x, 0, 0.1f);
        //deltaMovement.z = Mathf.Lerp(deltaMovement.z, 0, 0.1f);

        //print("x = " + deltaMovement.x + "; z = " + deltaMovement.z);

        debugGUI.SetText("Velocity = {0:1}", ply.velocity.sqrMagnitude);
          
        if (ply.isGrounded)
        {
            lastMove = deltaMovement;
            print(lastMove);
        }
        
        ply.Move(deltaMovement * Time.deltaTime);

        if (Input.GetButton("Reload"))
        {
            Vector3 curPos = headBone.transform.position;
            transform.position = SpawnPosition;
            verticalVelocity = 0f;
        }
    }
}