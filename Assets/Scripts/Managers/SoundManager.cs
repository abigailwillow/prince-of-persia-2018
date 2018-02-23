using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioList SoundList;
    public float SFXVolume = 0.5f;

    AudioSource SoundSource;
    FPSController ply;

    void Awake()
    {
        SoundSource = GetComponent<AudioSource>();
        if (GetComponent<FPSController>() != null)
        {
            ply = GetComponent<FPSController>();
        }
    }

    public void PlaySound(string RequestedSound)
    {
        foreach (AudioClip sound in SoundList.Sounds)
        {
            if (sound.name == RequestedSound)
            {
                if (ply != null)
                {
                    if (ply.isGrounded)
                    {
                        SoundSource.volume = Random.Range(0.5f, 0.6f);
                        SoundSource.pitch = Random.Range(0.75f, 1.25f);
                        SoundSource.PlayOneShot(sound, SFXVolume);
                    }
                }
                else
                {
                    SoundSource.volume = Random.Range(0.5f, 0.6f);
                    SoundSource.pitch = Random.Range(0.75f, 1.25f);
                    SoundSource.PlayOneShot(sound, SFXVolume);
                }
            }
        }
    }
}
