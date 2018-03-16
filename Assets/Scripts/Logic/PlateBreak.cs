using UnityEngine;
using System.Collections;

public class PlateBreak : MonoBehaviour
{
    public FallingPlate plate;

    void Awake()
    {
        plate = GetComponentInParent<FallingPlate>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > plate.MaxVelocity)
        {
            plate.HitGround = true;
        }
    }
}
