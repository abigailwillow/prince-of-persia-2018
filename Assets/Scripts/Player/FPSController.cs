using UnityEngine;
using TMPro;
using System.Collections;

public class FPSController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float runMultiplier = 1.5f;
    public float jumpStrength = 6f;
    public float mouseSensitivity = 2f;
    public float playerGravity = 20f;
    public float ReachDistance = 1f;
    public float ReachSmooth = 0.1f;
    public float ReachTime = 0.2f;
    //public float cameraSmooth = 0.1f;
    //public float maxCameraMove = 1f;
    public Transform headBone;
    public Camera FirstPersonCamera;
    public TextMeshProUGUI debugGUI;
    public Animator ViewmodelAnimator;
    public float HitCooldown = 1f;
    public float BlockCooldown = 0.75f;
    public float CrouchDelta = 0.75f;
    public HeartLocator HealthCanvas;
    public Sprite HeartEmpty;
    public GameObject PlayerRagdoll;
    public GameObject GameOverScreen;

    public static bool IsCursorLocked;
    public bool Attacking;
    public bool Blocking;
    public static int Health = 3;

    float movementH;
    float movementV;
    float rotationX;
    float rotationY;
    float curMultiplier = 0f;
    Vector3 curRotation = Vector3.zero;
    Animator anim;
    Vector3 lastMove;
    CharacterController ply;
    float NextHit = 0f;
    float NextBlock = 0f;
    bool DebugMode = false;

    [HideInInspector] public Vector3 deltaMovement = Vector3.zero;
    [HideInInspector] public bool Hanging;
    [HideInInspector] public bool isGrounded = true;
    [HideInInspector] public float verticalVelocity;

    void Awake()
    {
        ply = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        SetCursorLocked(true);
        headBone.transform.eulerAngles = curRotation;
        Health = 3;
    }

    public static void SetCursorLocked(bool boolean)
    {
        if (boolean)
        {
            Cursor.lockState = CursorLockMode.Locked;
            IsCursorLocked = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            IsCursorLocked = false;
        }
    }

    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) // Taking inputs
        {
            anim.SetFloat("MoveSpeed", 1f);
            ViewmodelAnimator.SetFloat("MoveSpeed", 1f);
            curMultiplier = 1f;
        }
        else
        {
            anim.SetFloat("MoveSpeed", 0f);
            ViewmodelAnimator.SetFloat("MoveSpeed", 0f);
            curMultiplier = 0f;
        }

        if (Input.GetAxisRaw("Vertical") > 0 && Input.GetButton("Sprint"))
        {
            anim.SetFloat("MoveSpeed", 2f);
            ViewmodelAnimator.SetFloat("MoveSpeed", 2f);
            curMultiplier = runMultiplier;
        }

        movementH = Input.GetAxisRaw("Horizontal");
        movementV = Input.GetAxisRaw("Vertical");

        if (IsCursorLocked)
        {
            rotationX = Input.GetAxis("Mouse X") * mouseSensitivity;
            rotationY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        }
        else
        {
            rotationX = 0;
            rotationY = 0;
        }

        if (!IsCursorLocked && Input.GetButtonDown("Fire1") && !PauseMenuManager.GamePaused)
        {
            SetCursorLocked(true);
        }

        curRotation.x -= rotationY;
        curRotation.x = Mathf.Clamp(curRotation.x, -90, 90);
        curRotation.y += rotationX;
        transform.eulerAngles = new Vector3(0, curRotation.y, 0);
        FirstPersonCamera.transform.eulerAngles = new Vector3(curRotation.x, transform.eulerAngles.y, 0);

        deltaMovement = new Vector3(movementH, 0, movementV);
        deltaMovement = Vector3.Normalize(deltaMovement);
        deltaMovement *= movementSpeed * curMultiplier;
        deltaMovement = transform.TransformDirection(deltaMovement);

        if (Input.GetButtonDown("Debug")) // Debug Mode
        {
            if (DebugMode)
            {
                DebugMode = false;
                movementSpeed *= 0.5f;
                verticalVelocity = 0f;
            }
            else
            {
                DebugMode = true;
                movementSpeed *= 2f;
                verticalVelocity = 0f;
                isGrounded = false;
            }
        }

        if (DebugMode)
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
            if (Hanging) // Hanging/climbing
            {
                isGrounded = Hanging;
            }

            if (ply.isGrounded != isGrounded && !Hanging)
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
        }

        //print("x = " + deltaMovement.x + "; z = " + deltaMovement.z);

        debugGUI.SetText("Velocity: {0:1} \nGrounded: " + isGrounded, ply.velocity.magnitude);

        ply.Move(deltaMovement * Time.deltaTime);

        if (Input.GetButtonDown("Fire1") && Time.time >= NextHit && !PauseMenuManager.GamePaused && !Blocking) // Attack
        {
            Attacking = true;
            NextHit = Time.time + HitCooldown;
            ViewmodelAnimator.SetTrigger("Attack");
            anim.SetTrigger("Attack");
        }

        if (Time.time >= NextHit)
        {
            Attacking = false;
        }

        if (Input.GetButtonDown("Fire2") && Time.time >= NextBlock && !PauseMenuManager.GamePaused && !Attacking) // Block
        {
            Blocking = true;
            NextBlock = Time.time + BlockCooldown;
            ViewmodelAnimator.SetTrigger("Block");
            anim.SetTrigger("Block");
        }

        if (Time.time >= NextBlock)
        {
            Blocking = false;
        }

        //print("attacking = " + Attacking + "; blocking = " + Blocking);

        if (Input.GetButtonDown("Crouch"))
        {
            BoxCollider HitBox = GetComponent<BoxCollider>();
            ply.height -= CrouchDelta;
            ply.center -= new Vector3(0, CrouchDelta / 2, 0);

            HitBox.size -= new Vector3(0, CrouchDelta, 0);
            HitBox.center -= new Vector3(0, CrouchDelta / 2, 0);

            FirstPersonCamera.transform.position -= new Vector3(0, CrouchDelta, 0);
        }

        if (Input.GetButtonUp("Crouch"))
        {
            BoxCollider HitBox = GetComponent<BoxCollider>();
            ply.height += CrouchDelta;
            ply.center += new Vector3(0, CrouchDelta / 2, 0);

            HitBox.size += new Vector3(0, CrouchDelta, 0);
            HitBox.center += new Vector3(0, CrouchDelta / 2, 0);

            FirstPersonCamera.transform.position += new Vector3(0, CrouchDelta, 0);
        }

        if (Input.GetButtonDown("Use"))
        {
            RaycastHit FoundItem;
            if (Physics.Raycast(FirstPersonCamera.transform.position, FirstPersonCamera.transform.forward, out FoundItem, ReachDistance))
            {
            }
            HitDeath();
        }
    }

    public void SetAttacking(bool AttackState)
    {
        Attacking = AttackState;
    }

    public void SetBlocking(bool BlockState)
    {
        Blocking = BlockState;
    }

    public void HitDamaged()
    {
        ViewmodelAnimator.SetTrigger("Knockback");
        switch (Health)
        {
            case 3:
                Health = 2;
                HealthCanvas.Heart_R.sprite = HeartEmpty;
                break;
            case 2:
                Health = 1;
                HealthCanvas.Heart_R.sprite = HeartEmpty;
                HealthCanvas.Heart_M.sprite = HeartEmpty;
                break;
            case 1:
                Health = 0;
                HitDeath();
                HealthCanvas.Heart_R.sprite = HeartEmpty;
                HealthCanvas.Heart_M.sprite = HeartEmpty;
                HealthCanvas.Heart_L.sprite = HeartEmpty;
                break;
            default:
                Health = 0;
                HitDeath();
                break;
        }
    }

    void HitDeath()
    {
        Health = 0;
        GameObject plyRagdoll = Instantiate(PlayerRagdoll, gameObject.transform.position, gameObject.transform.rotation);
        FirstPersonCamera.transform.SetParent(plyRagdoll.transform);
        for (int i = 0; i < FirstPersonCamera.transform.childCount; i++)
        {
            Destroy(FirstPersonCamera.transform.GetChild(i).gameObject);
        }
        FirstPersonCamera.GetComponent<DeathCamFollow>().DeathCam(plyRagdoll);
        SetCursorLocked(false);
        GameOverScreen.SetActive(true);
        gameObject.transform.gameObject.SetActive(false);
    }
}