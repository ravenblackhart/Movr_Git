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

    /*private void OnTriggerEnter(Collider other)
    {
        RegisterTape(other.gameObject);
    }*/
    

    public void RegisterTape(GameObject tape)
    {
        audioTracks = tape.GetComponent<Cassette>().TrackList; 
        trackIndex = Random.Range(0, audioTracks.Length);
        radioAudioSource.clip = audioTracks[trackIndex];
        //trackNameField.text = audioTracks[trackIndex].name;
    }

    public void UnregisterTape()
    {
        audioTracks = null; 
        //trackNameField.text = $"Insert Cassette";
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
        radioAudioSource.clip = audioTracks[index];
        trackNameField.text = audioTracks[index].name;
    }

}
