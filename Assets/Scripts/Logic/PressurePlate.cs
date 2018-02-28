using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Transform ControllingGate;
    public Transform PlateTransform;
    public float ActivatedOffset = 0.1f;
    public string ClickSound = "Plate_Click";

    GateController Gate;
    Vector3 Origin;
    SoundManager SoundOrigin;
    
    void Awake()
    {
        Gate = ControllingGate.GetComponentInChildren<GateController>();
        SoundOrigin = GetComponentInChildren<SoundManager>();
        Origin = PlateTransform.transform.localPosition;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Gate.Open();
            SoundOrigin.PlaySound(ClickSound);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player")
        {
            PlateTransform.transform.localPosition = Origin - new Vector3(0, ActivatedOffset, 0);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            PlateTransform.transform.localPosition = Origin;
            SoundOrigin.PlaySound(ClickSound);
        }
    }
}
