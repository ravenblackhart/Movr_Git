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
   private int playlistLength; 

   private void Awake()
   {
      audioSource = GetComponent<AudioSource>();
   }

   private void Start()
   {
      trackIndex = Random.Range(0, MainMenuMusic.PlaylistTracks.Length);
      audioSource.clip = MainMenuMusic.PlaylistTracks[trackIndex];
      playlistLength = MainMenuMusic.PlaylistTracks.Length; 
      StartCoroutine(PlayTape()); 
   }
   
   public void NextTrack()
   {
      trackIndex++;
      if (trackIndex > playlistLength - 1) trackIndex = 0; 
      UpdateTrack(trackIndex);
      StartCoroutine(PlayTape()); 
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
   }
}
