using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Audio;
using UnityEngine;

public class CassetteTest : MonoBehaviour
{
    [SerializeField] private Mixtape Cassette;

    public AudioClip[] TrackList;

    private void Start()
    {
        TrackList = Cassette.PlaylistTracks;
    }
}
