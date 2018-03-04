using UnityEngine;
using System.Collections;

public class EnemyRoomLogic : MonoBehaviour
{
    public AIController enemy;
    public Transform DoorObject;
    public float OpeningDistance = 2.5f;
    public float OpenTime = 1f;

    Vector3 DoorStartPos;
    bool LastAliveCheck;

    void Awake()
    {
        DoorStartPos = DoorObject.transform.localPosition;
    }

    void LateUpdate()
    {
        if (LastAliveCheck == true && enemy.enabled == false)
        {
            StartCoroutine(OpenRoom());
        }
        LastAliveCheck = enemy.enabled;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            enemy.PlayerInRoom = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            enemy.PlayerInRoom = false;
        }
    }

    IEnumerator OpenRoom()
    {
        float ElapsedOpenTime = 0f;
        while (ElapsedOpenTime < OpenTime)
        {
            DoorObject.transform.localPosition = Vector3.Lerp(DoorStartPos, new Vector3(DoorStartPos.x, OpeningDistance, DoorStartPos.z), ElapsedOpenTime / OpenTime);
            ElapsedOpenTime += Time.deltaTime;
            yield return null;
        }
    }
}
