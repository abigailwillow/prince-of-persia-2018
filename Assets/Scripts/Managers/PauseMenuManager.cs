using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public static bool GamePaused;
    public GameObject PauseCanvas;

    void Awake()
    {
        GamePaused = false;
        AudioListener.pause = false;
    }

	void Update()
    {
        if (Input.GetButtonDown("Menu") && FPSController.Health != 0)
        {
            if (!GamePaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void ResumeGame()
    {
        AudioListener.pause = false;
        FPSController.SetCursorLocked(true);
        GamePaused = false;
        Time.timeScale = 1f;
        PauseCanvas.SetActive(false);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        print("Shutdown pending...");
        Application.Quit();
    }

    public void PauseGame()
    {
        AudioListener.pause = true;
        GamePaused = true;
        FPSController.SetCursorLocked(false);
        Time.timeScale = 0f;
        PauseCanvas.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
