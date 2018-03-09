using UnityEngine;
using System.Collections;

public class SpikeController : MonoBehaviour
{
    public Vector3 StartPos = new Vector3 (0, -2, 0);
    public float TriggerRange = 1f;
    public float DeployTime = 0.1f;
    public float HideTime = 0.5f;

    Transform ply;
    bool LastCheck;
    bool InRange;
    Vector3 SpikeVelocity;

	void Awake()
    {
        transform.localPosition = StartPos;
        ply = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        float diffX = ply.position.x - transform.position.x;
        float diffZ = ply.position.z - transform.position.z;
        if (diffX < TriggerRange && -diffX < TriggerRange && diffZ < TriggerRange && -diffZ < TriggerRange)
        {
            InRange = true;
        }
        else
        {
            InRange = false;
        }

        if (InRange && !LastCheck)
        {
            StartCoroutine(RaiseSpikes());
        }
        else if(!InRange && LastCheck)
        {
            StartCoroutine(LowerSpikes());
        }
        LastCheck = InRange;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            FPSController plyc = collider.GetComponent<FPSController>();
            if (!plyc.isGrounded)
            {
                plyc.HitDamaged(3);
            }
        }
    }

    IEnumerator RaiseSpikes()
    {
        float ElapsedTime = 0f;
        while (ElapsedTime < DeployTime)
        {
            transform.localPosition = Vector3.Lerp(StartPos, Vector3.zero, ElapsedTime / DeployTime);
            ElapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return null;
        transform.localPosition = Vector3.zero;
    }

    IEnumerator LowerSpikes()
    {
        float ElapsedTime = 0f;
        while (ElapsedTime < HideTime)
        {
            transform.localPosition = Vector3.Lerp(Vector3.zero, StartPos, ElapsedTime / HideTime);
            ElapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return null;
        transform.localPosition = StartPos;
    }
}
