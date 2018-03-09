using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenuManager : MonoBehaviour
{
	public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
