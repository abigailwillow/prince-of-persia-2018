using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    public Transform DoorObject;
    public float OpeningDistance = 2.5f;
    public float OpenTime = 1f;

    Vector3 StartPos;
    bool PlayerInside;

    void Awake()
    {
        StartPos = DoorObject.transform.localPosition;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            PlayerInside = true;
            StartCoroutine(OpenDoor());
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            PlayerInside = false;
            StartCoroutine(CloseDoor());
        }
    }

    IEnumerator OpenDoor()
    {
        Vector3 Origin = DoorObject.transform.localPosition;
        float ElapsedOpenTime = 0f;
        while (ElapsedOpenTime < OpenTime && PlayerInside)
        {
            DoorObject.transform.localPosition = Vector3.Lerp(Origin, new Vector3(Origin.x, OpeningDistance, Origin.z), ElapsedOpenTime / OpenTime);
            ElapsedOpenTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator CloseDoor()
    {
        Vector3 Origin = DoorObject.transform.localPosition;
        float ElapsedCloseTime = 0f;
        while (ElapsedCloseTime < OpenTime && !PlayerInside)
        {
            DoorObject.transform.localPosition = Vector3.Lerp(Origin, StartPos, ElapsedCloseTime / OpenTime);
            ElapsedCloseTime += Time.deltaTime;
            yield return null;
        }
    }
}
