using UnityEngine;

public class TorchSoundScript : MonoBehaviour
{
    AudioSource Sound;
    bool IsPlaying = false;

    void Awake()
    {
        Sound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!IsPlaying && Time.frameCount % 10 == 0)
        {
            int RandomNum = Random.Range(0, 100);
            if (RandomNum < 10)
            {
                IsPlaying = true;
                Sound.pitch = Random.Range(0.4f, 0.6f);
                Sound.loop = true;
                Sound.Play();
            }
        }
    }
}
