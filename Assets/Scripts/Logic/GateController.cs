using UnityEngine;
using System.Collections;

public class GateController : MonoBehaviour
{
    public float TimeToOpen = 5f;
    public float TimeToClose = 10f;
    public float KeepOpenTime = 5f;
    public float OpeningDistance = 2.9f;

    public AudioClip GateOpenSound;
    public AudioClip GateCloseSound;
    //public float GateSmoothing = 0.1f;

    float ElapsedTimeToOpen;
    float ElapsedTimeToClose;
    bool IsOpening = false;
    Vector3 Origin;
    AudioSource SoundSource;

    void Awake()
    {
        Origin = transform.localPosition;
        SoundSource = GetComponent<AudioSource>();
    }

	public void Open()
    {
        if (!IsOpening)
        {
            StartCoroutine(OpenGate(false));
        }
    }

    public void StayOpen()
    {
        if (!IsOpening)
        {
            StartCoroutine(OpenGate(true));
        }
    }

    IEnumerator OpenGate(bool Stay)
    {
        IsOpening = true;
        SoundSource.PlayOneShot(GateOpenSound);
        ElapsedTimeToOpen = 0f;
        while (ElapsedTimeToOpen < TimeToOpen)
        {
            transform.localPosition = Vector3.Lerp(Origin, Origin + new Vector3(0, OpeningDistance, 0), ElapsedTimeToOpen / TimeToOpen);
            //print("OpenStage: " + ElapsedTimeToOpen / TimeToOpen);
            ElapsedTimeToOpen += Time.deltaTime;
            yield return null;
        }
        yield return null;
        if (!Stay)
        {
            StartCoroutine(KeepOpen());
        }
    }

    IEnumerator KeepOpen()
    {
        yield return new WaitForSeconds(KeepOpenTime);
        StartCoroutine(CloseGate());
    }

    IEnumerator CloseGate()
    {
        SoundSource.PlayOneShot(GateCloseSound);
        ElapsedTimeToClose = 0f;
        while (ElapsedTimeToClose < TimeToClose)
        {
            transform.localPosition = Vector3.Lerp(Origin + new Vector3(0, OpeningDistance, 0), Origin, ElapsedTimeToClose / TimeToClose);
            //print("CloseStage: " + ElapsedTimeToClose / TimeToClose);
            ElapsedTimeToClose += Time.deltaTime;
            yield return null;
        }
        IsOpening = false;
    }
}
