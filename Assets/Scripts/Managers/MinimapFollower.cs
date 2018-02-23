using UnityEngine;

public class MinimapFollower : MonoBehaviour {

    float StartHeight;

    void Awake()
    {
        StartHeight = transform.position.y;
    }

    void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, StartHeight, transform.position.z);
    }
}
