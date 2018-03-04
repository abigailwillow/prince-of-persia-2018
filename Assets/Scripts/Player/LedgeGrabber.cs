using UnityEngine;
using System.Collections;

public class LedgeGrabber : MonoBehaviour
{
    public float GrabDistance = 1f;
    public float HangTime = 1f;
    public float DetachForce = 5f;
    public float PullupTime = 1f;
    public float FixOffset = 0.2f;
    public Camera FirstPersonCamera;
    public Animator anim;

    FPSController ply;
    Vector3 Origin;
    //CharacterController plycon;

    void Awake()
    {
        ply = GetComponent<FPSController>();
        //plycon = ply.GetComponent<CharacterController>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Grabbable")
        {
            RaycastHit WallCheck;
            if (Physics.Raycast(FirstPersonCamera.transform.position, FirstPersonCamera.transform.forward, out WallCheck, GrabDistance))
            {
                if (WallCheck.transform.tag == "Grabbable" && !ply.isGrounded)
                {
                    Vector3 LedgeTarget = WallCheck.transform.Find("GrabTarget").transform.position;
                    StartCoroutine(LedgeRoutine(LedgeTarget));
                }
            }
        }
    }

    IEnumerator LedgeRoutine(Vector3 LedgeTarget)
    {
        float ElapsedTime = 0f;
        Origin = transform.position;
        while (ElapsedTime < HangTime)
        {
            if (Input.GetAxis("Vertical") < 0)
            {
                ply.Hanging = false;
                break;
            }
            else if (Input.GetButtonDown("Jump")) 
            {
                ply.Hanging = false;
                StartCoroutine(GrabRoutine(LedgeTarget));
                break;
            }
            else
            {
                ply.verticalVelocity = 0f;
                ply.deltaMovement = Vector3.zero;
                ply.Hanging = true;
                ElapsedTime += Time.deltaTime;
            }
            yield return null;
            ply.Hanging = false;
        }
    }

    IEnumerator GrabRoutine(Vector3 LedgeTarget)
    {
        Vector3 SecondOrigin = transform.position;
        anim.SetTrigger("Climb");
        for (float i = 0f; i < PullupTime; i += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(SecondOrigin, new Vector3(transform.position.x, LedgeTarget.y, transform.position.z), i / PullupTime);
            ply.deltaMovement = Vector3.zero;
            ply.verticalVelocity = 0f;
            yield return null;
        }
        yield return null;
        ply.verticalVelocity = 0f;
        transform.position = new Vector3(transform.position.x + FixOffset, LedgeTarget.y, transform.position.z);
    }

    void LateUpdate()
    {
        if (ply.Hanging)
        {
            transform.position = Origin;
        }
        anim.SetBool("Hanging", ply.Hanging);
    }
}