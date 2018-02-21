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
    //public float cameraSmooth = 0.1f;
    //public float maxCameraMove = 1f;
    public Transform headBone;
    public Camera FirstPersonCamera;
    public LedgeGrabber hangScript;
    public TextMeshProUGUI debugGUI;
    public float SFXVolume = 0.5f;
    public AudioClip StoneFootstep;

    float movementH;
    float movementV;
    float rotationX;
    float rotationY;
    float curMultiplier = 0f;
    Vector3 curRotation = Vector3.zero;
    Vector3 deltaMovement = Vector3.zero;
    Animator anim;
    Vector3 lastMove;
    ButtonController ButtonScript;
    bool isCursorLocked;
    CharacterController ply;
    AudioSource audioSource;

    [HideInInspector]
    public bool isGrounded = true;
    [HideInInspector]
    public float verticalVelocity;

    void Awake()
    {
        ply = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        ButtonScript = GetComponent<ButtonController>();
        audioSource = GetComponent<AudioSource>();
        setCursorLocked(true);
        headBone.transform.eulerAngles = curRotation;
    }

    public void setCursorLocked(bool boolean)
    {
        if (boolean)
        {
            Cursor.lockState = CursorLockMode.Locked;
            isCursorLocked = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            isCursorLocked = false;
        }
    }

    void LateUpdate()
    {
        headBone.transform.eulerAngles += new Vector3(curRotation.x, 0, 0);
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
            anim.SetFloat("MoveSpeed", 2f);
            curMultiplier = runMultiplier;
        }
        movementH = Input.GetAxisRaw("Horizontal");
        movementV = Input.GetAxisRaw("Vertical");

        if (isCursorLocked)
        {
            rotationX = Input.GetAxis("Mouse X") * mouseSensitivity;
            rotationY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        }
        else
        {
            rotationX = 0;
            rotationY = 0;
        }

        curRotation.x -= rotationY;
        curRotation.x = Mathf.Clamp(curRotation.x, -90, 90);
        //curRotation.y = transform.eulerAngles.y;
        curRotation.y += rotationX;
        //FirstPersonCamera.transform.eulerAngles = curRotation;
        //transform.Rotate(0, rotationX, 0); // Body rotation
        transform.eulerAngles = new Vector3(0, curRotation.y, 0);

        deltaMovement = new Vector3(movementH, 0, movementV);
        deltaMovement = Vector3.Normalize(deltaMovement);
        deltaMovement *= movementSpeed * curMultiplier;
        deltaMovement = transform.TransformDirection(deltaMovement);

        if (Input.GetButtonDown("Debug")) // Debug Mode
        {
            if (ButtonScript.debugMode)
            {
                ButtonScript.debugMode = false;
                movementSpeed *= 0.5f;
                verticalVelocity = 0f;
            }
            else
            {
                ButtonScript.debugMode = true;
                movementSpeed *= 2f;
                verticalVelocity = 0f;
                isGrounded = false;
            }
        }

        if (ButtonScript.debugMode)
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

        //print("x = " + deltaMovement.x + "; z = " + deltaMovement.z);

        debugGUI.SetText("Velocity: {0:1} \nGrounded: " + isGrounded, ply.velocity.magnitude);

        ply.Move(deltaMovement * Time.deltaTime);

        if (Input.GetButtonDown("Fire1"))
        {
            // ATTACK!
        }
    }

    void PlayFootstep()
    {
        if (isGrounded)
        {
            audioSource.volume = Random.Range(0.5f, 0.6f);
            audioSource.pitch = Random.Range(0.75f, 1.25f);
            audioSource.PlayOneShot(StoneFootstep, SFXVolume);
        }
    }
}