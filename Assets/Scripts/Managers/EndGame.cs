using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
	void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            FPSController.SetCursorLocked(false);
            SceneManager.LoadScene("Credits");
        }
    }
}
