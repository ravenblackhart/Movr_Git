using System;
using _Scripts.Audio;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioMixer Mixer;

    [Tooltip("For Settings Panel")] public AudioSetting[] AudioSettings;
    private enum AudioGroups {Global, Music, SFX, Environment }
    
    [Tooltip("In-game Audio")] public Sound[] sounds;

   

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
            s.source.outputAudioMixerGroup = s.MixerGroup;  
            
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
    [CanBeNull] public Slider slider;
    public string exposedParam;

    [SerializeField][Range(-80f, 0f)] private float audioVolume; 
    public Toggle soundToggle;
    private Image audioStrength; 

    public void Initialize()
    {
        audioVolume = PlayerPrefs.GetFloat(exposedParam);
        if (slider != null) slider.value = audioVolume;

        audioStrength = soundToggle.transform.Find("VolImage").GetComponent<Image>(); 

    }

    void OnValidate()
    {
        SetExposedParam(audioVolume);
        if (audioVolume > 60f) 
            audioStrength.sprite = UIManager.Instance.AudioStrength3; 
        if (audioVolume <= 60f) 
            audioStrength.sprite = UIManager.Instance.AudioStrength2; 
        if (audioVolume <= 30f) 
            audioStrength.sprite = UIManager.Instance.AudioStrength1;
    }

    public void ToggleAudio()
    {
        if (soundToggle)
        {
            slider.value = PlayerPrefs.GetFloat(exposedParam);
            
        }
        
        if (!soundToggle)
        {
            slider.value = slider.minValue;
            soundToggle.image.color = UIManager.Instance.DisabledColor; 
        }
    }
    
    public void SetExposedParam(float value)
    {
        //redX.SetActive(value <= slider.minValue);
        AudioManager.Instance.Mixer.SetFloat(exposedParam, value);
        PlayerPrefs.SetFloat(exposedParam, value);
    }
}