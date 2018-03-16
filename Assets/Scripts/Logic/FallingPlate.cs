using UnityEngine;
using System.Collections;

public class FallingPlate : MonoBehaviour
{
    public GameObject BrokenPlate;
    public GameObject PlateObject;
    public Vector3 PlateOffset = new Vector3(0, -0.1f, 0);
    public float SafeTime = 1f;
    public float MaxVelocity = 2f;
    public string WarnSound = "Plate_Click";
    public float MaxForce = 10f;
    public bool HitGround;

    //bool PlayerOnPlate;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            StartCoroutine(PlateFall());
            SoundManager SM = PlateObject.GetComponent<SoundManager>();
            if (SM.enabled && !PlateObject.GetComponent<Rigidbody>().useGravity)
            {
                SM.PlaySound(WarnSound);
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            PlateObject.transform.position -= PlateOffset;
            SoundManager SM = PlateObject.GetComponent<SoundManager>();
            if (SM.enabled && !PlateObject.GetComponent<Rigidbody>().useGravity)
            {
                SM.PlaySound(WarnSound);
            }
        }
    }

    IEnumerator PlateFall()
    {
        PlateObject.transform.position += PlateOffset;
        yield return new WaitForSeconds(SafeTime);
        DropPlate();
    }

    void DropPlate()
    {
        PlateObject.GetComponent<Rigidbody>().useGravity = true;
    }

    void Update()
    {
        if (HitGround)
        {
            GameObject BPlate = Instantiate(BrokenPlate, PlateObject.transform.position, PlateObject.transform.rotation, transform);
            PlateObject.SetActive(false);
            Rigidbody[] RBArray = BPlate.transform.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rbi in RBArray)
            {
                rbi.AddForce(new Vector3(Random.Range(MaxForce, MaxForce), Random.Range(0, MaxForce), Random.Range(MaxForce, MaxForce)));
            }
            HitGround = false;
        }
    }

}
