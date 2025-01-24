using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.UIElements;

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

        PlayAudio("BGM", true);
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
    public void PlayAudio(string name, bool loop)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            return;
        }

        //Debug.Log(name + " : " + s.clip);

        s.source.loop = loop;
        s.source.Play();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayAudio("Death", false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayAudio("Respawn", false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StopAudio("BGM");
            PlayAudio("Victory", false);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            StopAudio("BGM");
            PlayAudio("Defeat", false);
        }


        if (Input.GetKeyDown(KeyCode.Y))
        {
            PlayAudio("Menu Navigation", false);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            PlayAudio("Menu Select", false);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayAudio("Menu Back", false);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayAudio("Menu Invalid", false);
        }
    }
}
