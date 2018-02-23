using UnityEngine;

[CreateAssetMenu(fileName = "New Soundlist", menuName = "SoundList")]
public class AudioList : ScriptableObject
{
    public AudioClip[] Sounds;
}
