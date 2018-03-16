using UnityEngine;
using System.Collections;

public class EnemyRoomLogic : MonoBehaviour
{
    public AIController enemy;
    public GateController Gate;

    bool LastAliveCheck;

    void LateUpdate()
    {
        if (LastAliveCheck == true && enemy.enabled == false)
        {
            Gate.StayOpen();
        }
        LastAliveCheck = enemy.enabled;
    }

    void OnTriggerStay(Collider collider)
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
}
