using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Audio;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class MainMenuAudio : MonoBehaviour
{
   [SerializeField] private Mixtape MainMenuMusic;
   private AudioSource audioSource; 
   private int trackIndex; 

   private void Awake()
   {
      audioSource = GetComponent<AudioSource>();
   }

   private void Start()
   {
      trackIndex = Random.Range(0, MainMenuMusic.PlaylistTracks.Length);
      audioSource.clip = MainMenuMusic.PlaylistTracks[trackIndex];
      StartCoroutine(PlayTape()); 
   }
   
   public void NextTrack()
   {
      trackIndex++;
      if (trackIndex > MainMenuMusic.PlaylistTracks.Length - 1) trackIndex = 0; 
      UpdateTrack(trackIndex);
   }
   
   private void UpdateTrack(int index)
   {
      audioSource.clip = MainMenuMusic.PlaylistTracks[index];
   }
   
   private IEnumerator PlayTape()
   {
      audioSource.Play();
      yield return new WaitForSeconds(MainMenuMusic.PlaylistTracks[trackIndex].length + 0.15f); 
      NextTrack();
      audioSource.Play();
   }
}
