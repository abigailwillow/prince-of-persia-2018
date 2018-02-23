using UnityEngine;
using System.Collections;

public class HintZoneManager : MonoBehaviour
{
    public GameObject CanvasTemplate;
    public GameObject HUDCanvas;
    public string ActionName;
    public string ActionButton;
    public float LifeTime = 5f;
    public float FadeTime = 1f;

    HintTextLocator HintElements;

    void OnTriggerEnter(Collider collider)
    { 
        if (collider.tag == "Player")
        {
            StartCoroutine(FadeCanvas());
        }
    }

    IEnumerator FadeCanvas()
    {
        GameObject HintCanvas = Instantiate(CanvasTemplate, HUDCanvas.transform, false);
        HintElements = HintCanvas.GetComponent<HintTextLocator>();
        HintElements.ActionText.text = ActionName;
        HintElements.ActionButton.text = ActionButton;
        CanvasGroup FadingCanvas = HintCanvas.GetComponent<CanvasGroup>();

        for (float i = 0; i <= FadeTime; i += Time.deltaTime)
        {
            FadingCanvas.alpha = i;
            yield return null;
        }

        yield return new WaitForSeconds(LifeTime);

        for (float i = FadeTime; i >= 0; i -= Time.deltaTime)
        {
            FadingCanvas.alpha = i;
            yield return null;
        }

        yield return null;

        Destroy(HintCanvas);
    }
}
