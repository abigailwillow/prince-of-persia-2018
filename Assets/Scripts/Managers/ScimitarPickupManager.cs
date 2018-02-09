using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScimitarPickupManager : MonoBehaviour {

    public float VerticalSpeed = 3f;
    public float RotationSpeed = 50f;
    public float VerticalRange = 0.1f;

    Vector3 StartPos;

    void Awake()
    {
        StartPos = transform.position;
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, -Time.time * RotationSpeed, 0);
        transform.position = new Vector3(StartPos.x, StartPos.y + Mathf.Sin(Time.time * VerticalSpeed) * VerticalRange, StartPos.z);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            gameObject.SetActive(false);
        }
    }
}
