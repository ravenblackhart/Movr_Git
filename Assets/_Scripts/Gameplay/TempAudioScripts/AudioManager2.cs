using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager2 : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private AudioSource _clip;
    private void Awake()
    {
        // if (instance == null)
        //     instance = this;
        // else
        // {
        //     Destroy(gameObject);
        //     return;
        // }
        //
        // DontDestroyOnLoad(gameObject);
        
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        //_clip = GetComponent<AudioSource>();
        //_clip.Play();
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Play();
        }
        // // if (!s.source.isPlaying)
        // // {
        //     s.source.Play();
        // // }
    }
}
