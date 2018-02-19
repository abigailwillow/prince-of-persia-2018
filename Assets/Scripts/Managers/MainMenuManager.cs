using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.PostProcessing;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour {

    public TMP_Dropdown QualityDropdown;
    public TMP_Dropdown ResolutionDropdown;
    public Toggle FullscreenToggle;
    public Toggle AAToggle;
    public Toggle ReflectionToggle;
    public Toggle AOToggle;
    public AudioMixer audioMixer;
    public PostProcessingProfile VideoSettings;

    Resolution[] AvailableResolutions;

    void Awake()
    {
        List<string> QualityLevels = new List<string>(QualitySettings.names);
        QualityDropdown.AddOptions(QualityLevels);
        QualityDropdown.value = QualitySettings.GetQualityLevel();

        AvailableResolutions = Screen.resolutions;
        List<string> ResolutionList = new List<string>();

        for (int i = 0; i < AvailableResolutions.Length; i++)
        {
            if (!ResolutionList.Contains(AvailableResolutions[i].width + " x " + AvailableResolutions[i].height))
            ResolutionList.Add(AvailableResolutions[i].width + " x " + AvailableResolutions[i].height);
        }
        ResolutionList.Sort();
        ResolutionDropdown.AddOptions(ResolutionList);

        FullscreenToggle.isOn = Screen.fullScreen;
        AAToggle.isOn = VideoSettings.antialiasing.enabled;

        ReflectionToggle.isOn = VideoSettings.screenSpaceReflection.enabled;

        AOToggle.isOn = VideoSettings.ambientOcclusion.enabled;
    }

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
        Volume = CalculateDB(Volume);
        audioMixer.SetFloat("MasterVolume", Volume);
    }

    public void SetMusicVolume(float Volume)
    {
        Volume = CalculateDB(Volume);
        audioMixer.SetFloat("MusicVolume", Volume);
    }

    public void SetSoundVolume(float Volume)
    {
        Volume = CalculateDB(Volume);
        audioMixer.SetFloat("SoundVolume", Volume);
    }

    public float CalculateDB(float Volume)
    {
        Volume = Mathf.Pow(0.8659f, Volume) * -80f;
        return Volume;
    }

    // Video settings
    public void SetQuality(int QualityIndex)
    {
        QualitySettings.SetQualityLevel(QualityIndex, true);
    }

    public void SetResolution()
    {
        TextMeshProUGUI SelectedText = ResolutionDropdown.GetComponentInChildren<TextMeshProUGUI>();
        string[] ResolutionArray = SelectedText.text.Split(' ');
        int resolutionWidth = System.Convert.ToInt32(ResolutionArray[0]);
        int resolutionHeight = System.Convert.ToInt32(ResolutionArray[2]);

        Screen.SetResolution(resolutionWidth, resolutionHeight, Screen.fullScreen);
    }

    public void ToggleFullscreen(bool FullState)
    {
        Screen.fullScreen = FullState;
    }

    public void ToggleAA(bool AAState)
    {
        VideoSettings.antialiasing.enabled = AAState;
    }

    public void ToggleReflection(bool ReflectionState)
    {
        VideoSettings.screenSpaceReflection.enabled = ReflectionState;
    }

    public void ToggleAO(bool AOState)
    {
        VideoSettings.ambientOcclusion.enabled = AOState;
    }
}
