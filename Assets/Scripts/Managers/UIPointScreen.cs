using UnityEngine;

public class UIPointScreen : MonoBehaviour
{
    Transform MainCam;

    void Awake()
    {
        MainCam = Camera.main.transform;
    }
    void LateUpdate()
    {
        if (FPSController.Health != 0)
        {
            transform.rotation = MainCam.rotation;
        }
    }
}
