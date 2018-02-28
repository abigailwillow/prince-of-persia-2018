using UnityEngine;
using System.Collections;

public class CameraBobbing : MonoBehaviour
{
    public Camera PlayerCam;
    public float BobSmooth = 0.1f;
    public float BobSpeed = 0.2f;
    public float BobStrength = 0.1f;

	public void SingleBob()
    { 
        StartCoroutine(CameraBob());
    }

    void Update()
    {
        if (Input.GetButtonDown("Reload"))
        {
            print("bitch");
            SingleBob();
        }
    }

    IEnumerator CameraBob()
    {
        for (float i = 0f; i < BobSpeed; i += Time.deltaTime)
        {
            PlayerCam.transform.localPosition += Vector3.Lerp(Vector3.zero, new Vector3(0, -BobStrength, 0), BobSmooth);
            yield return null;
        }
        yield return null;
        for (float i = 0f; i < BobSpeed; i += Time.deltaTime)
        {
            PlayerCam.transform.localPosition += Vector3.Lerp(Vector3.zero, new Vector3(0, BobStrength, 0), BobSmooth);
            yield return null;
        }
    }
}
