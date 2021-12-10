// Author: Fatima Nadeem / Croft

using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume; // volume
    public bool loop; // Bool to dictate if this sound will loop

    [HideInInspector]
    public AudioSource audioSrc;
}
