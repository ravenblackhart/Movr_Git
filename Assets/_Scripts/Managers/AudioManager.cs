using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioManager : MonoBehaviour
{
    public AudioMixer Mixer;
    public AudioSetting[] AudioSettings; 
    private enum AudioGroups {Global, Music, SFX, Environment }
    public Sound[] sounds;

    public static AudioManager Instance;

    private AudioSource _clip;
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        for (int i = 0; i < AudioSettings.Length; i++)
        {
            AudioSettings[i].Initialize();
        }
    }

    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null) {
            s.source.Play();
        }
        // // if (!s.source.isPlaying)
        // // {
        //     s.source.Play();
        // // }
    }

    public void GlobalSlider(float value)
    {
        AudioSettings[(int)AudioGroups.Global].SetExposedParam(value); 
    }
    
    public void MusicSlider(float value)
    {
        AudioSettings[(int)AudioGroups.Music].SetExposedParam(value); 
    }
    
    public void SFXSlider(float value)
    {
        AudioSettings[(int)AudioGroups.SFX].SetExposedParam(value); 
    }
    public void EnvSlider(float value)
    {
        AudioSettings[(int)AudioGroups.Environment].SetExposedParam(value); 
    }

    public void GlobalToggle()
    {
        AudioSettings[(int)AudioGroups.Global].ToggleAudio();
    }
    
    public void MusicToggle()
    {
        AudioSettings[(int)AudioGroups.Music].ToggleAudio();
    }
    
    public void SFXToggle()
    {
        AudioSettings[(int)AudioGroups.SFX].ToggleAudio();
    }
    
    public void EnvToggle()
    {
        AudioSettings[(int)AudioGroups.Environment].ToggleAudio();
    }
    
    
}

[System.Serializable]
public class AudioSetting
{
    public Slider slider;
    public GameObject redX;
    public string exposedParam;
    public Toggle soundToggle; 

    public void Initialize()
    {
        slider.value = PlayerPrefs.GetFloat(exposedParam);
    }

    public void ToggleAudio()
    {
        if (!soundToggle)
        {
            slider.value = PlayerPrefs.GetFloat(exposedParam);
        }
        
        if (soundToggle)
        {
            slider.value = slider.minValue; 
        }
    }
    
    public void SetExposedParam(float value)
    {
        redX.SetActive(value <= slider.minValue);
        AudioManager.Instance.Mixer.SetFloat(exposedParam, value);
        PlayerPrefs.SetFloat(exposedParam, value);
    }
}