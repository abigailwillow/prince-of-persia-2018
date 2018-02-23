using UnityEngine;

public class LedgeGrabber : MonoBehaviour
{ 
    [HideInInspector]
    public bool Hanging = false;

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Grabbable")
        {
            Hanging = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Grabbable")
        {
            Hanging = false;
        }
    }
}