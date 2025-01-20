using UnityEngine;
using System;
using UnityEngine.Audio;

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
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

        PlayAudio("BGM");
    }

    public void StopAudio(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            return;
        }
        s.source.Stop();
    }
    public void PlayAudio(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            return;
        }

        Debug.Log(name + " : " + s.clip);

        s.source.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayAudio("Death");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayAudio("Respawn");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StopAudio("BGM");
            PlayAudio("Victory");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            StopAudio("BGM");
            PlayAudio("Defeat");
        }


        if (Input.GetKeyDown(KeyCode.Y))
        {
            PlayAudio("Menu Navigation");
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            PlayAudio("Menu Select");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayAudio("Menu Back");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayAudio("Menu Invalid");
        }
    }
}
