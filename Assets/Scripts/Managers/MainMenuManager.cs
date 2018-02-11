using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public void LoadScene(string Scene)
    {
        SceneManager.LoadScene(Scene);
    }

    public void OpenURL(string WebURL)
    {
        Application.OpenURL(WebURL);
    }

    public void QuitApplication()
    {
        print("Shutdown Pending!");
        Application.Quit();
    }
}
