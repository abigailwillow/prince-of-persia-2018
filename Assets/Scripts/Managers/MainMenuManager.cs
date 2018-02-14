using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenuManager : MonoBehaviour {

    public AudioMixer audioMixer;

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

    public void SetMasterVolume(float Volume)
    {
        audioMixer.SetFloat("MasterVolume", Volume);
    }

    public void SetMusicVolume(float Volume)
    {
        audioMixer.SetFloat("MusicVolume", Volume);
    }

    public void SetSoundVolume(float Volume)
    {
        audioMixer.SetFloat("SoundVolume", Volume);
    }
}
