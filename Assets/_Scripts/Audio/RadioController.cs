using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Audio;
using TMPro;
using UnityEngine;

public class RadioController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI trackNameField; 

    [Header("Track List")] 
    [SerializeField] private Track[] audioTracks;

    private int trackIndex; 
    private AudioSource radioAudioSource;

    private void Start()
    {
        radioAudioSource = GetComponent<AudioSource>();
        trackIndex = 0;
        radioAudioSource.clip = audioTracks[trackIndex].TrackAudioClip;
        trackNameField.text = audioTracks[trackIndex].name; 
    }

    public void PlayAudio()
    {
        radioAudioSource.Play();
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
        UpdateTrack(trackIndex);
    }
    
    public void PrevTrack()
    {
        trackIndex--; 
        UpdateTrack(trackIndex);
    }

    private void UpdateTrack(int index)
    {
        radioAudioSource.clip = audioTracks[index].TrackAudioClip;
        trackNameField.text = audioTracks[index].name;
    }

}
