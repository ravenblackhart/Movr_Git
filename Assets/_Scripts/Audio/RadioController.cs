using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Audio;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RadioController : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioTracks;

    private int trackIndex; 
    private AudioSource radioAudioSource;
    private float timeStamp = 0f; 

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
        radioAudioSource.time = Random.Range(0f, audioTracks[trackIndex].length);
        timeStamp = radioAudioSource.time; 

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
        UnregisterTape();
    }

    public void NextTrack()
    {
        if (audioTracks != null)
        {
            trackIndex++;
            if (trackIndex > audioTracks.Length - 1)
            {
                trackIndex = 0;
            } 
            UpdateTrack(trackIndex);
        }
        
        PlayAudio();
    }

    private void UpdateTrack(int index)
    {
        radioAudioSource.clip = audioTracks[index];
        timeStamp = 0f;
        radioAudioSource.time = timeStamp; 
    }

    private IEnumerator PlayTape()
    {
        radioAudioSource.Play();
        yield return new WaitForSeconds(audioTracks[trackIndex].length - timeStamp); 
        NextTrack();
    }

}
