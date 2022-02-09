using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Audio;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RadioController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI trackNameField;
    [SerializeField] private AudioClip[] audioTracks;

    private int trackIndex; 
    private AudioSource radioAudioSource;

    private void Start()
    {
        radioAudioSource = GetComponent<AudioSource>();
        UnregisterTape();
    }
    
    
    
    

    public void RegisterTape(GameObject tape)
    {
        audioTracks = tape.GetComponent<Cassette>().TrackList; 
        trackIndex = Random.Range(0, audioTracks.Length);
        radioAudioSource.clip = audioTracks[trackIndex];
    }

    public void UnregisterTape()
    {
        audioTracks = null;
    }
    
    public void PlayAudio()
    {
        StartCoroutine(PlayTape());
    }

    public void PauseAudio()
    {
        radioAudioSource.Pause();
    }

    public void StopAudio()
    {
        radioAudioSource.Stop();
    }

    public void NextTrack()
    {
        trackIndex++;
        if (trackIndex > audioTracks.Length - 1) trackIndex = 0; 
        UpdateTrack(trackIndex);
    }
    
    public void PrevTrack()
    {
        trackIndex--; 
        if (trackIndex < 0 ) trackIndex = audioTracks.Length - 1 ;
        UpdateTrack(trackIndex);
    }

    private void UpdateTrack(int index)
    {
        radioAudioSource.clip = audioTracks[index];
        trackNameField.text = audioTracks[index].name;
    }

    private IEnumerator PlayTape()
    {
        radioAudioSource.time = Random.Range(0f, audioTracks[trackIndex].length);
        radioAudioSource.Play();
        yield return new WaitForSeconds(audioTracks[trackIndex].length + 0.15f); 
        NextTrack();
        radioAudioSource.Play();
    }

}
