// Author: Fatima Nadeem / Croft

using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    public Sound[] sounds;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        foreach (Sound s in sounds)
        {
            // Initialising audio source of sound
            s.audioSrc = gameObject.AddComponent<AudioSource>();
            s.audioSrc.clip = s.clip;
            s.audioSrc.volume = s.volume;
            s.audioSrc.loop = s.loop;
        }
    }

    public void Play(string name)
    /* This function plays the sound of the given name.
    */
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s != null)
        {
            s.audioSrc.Play();
            return;
        }
    }

    public void Stop(string name)
    /* This function stops the sound of the given name.
    */
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s != null)
        {
            s.audioSrc.Stop();
            return;
        }
    }

    public bool IsPlaying(string name)
    /* This function returns true if the sound of the given name is playing
     * else false.
    */
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s != null)
        {
            return s.audioSrc.isPlaying;
        }

        return false;
    }
}
