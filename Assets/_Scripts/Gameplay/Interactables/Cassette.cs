using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cassette : PhysicsObject
{
    //[SerializeField] private _Scripts.Audio.Mixtape Cassette;
    [SerializeField] private _Scripts.Audio.Mixtape Mixtape;
    public AudioClip[] TrackList;

    public Transform _targetPos;
    public string _musicGenre;

    public bool _sliding;
    public Transform _prevParent;
    CassettePlayer _cassettePlayer;

    private void Start() {
        TrackList = Mixtape.PlaylistTracks;
        _prevParent = transform.parent;
        _musicGenre = Mixtape.name;
    }

    public void SlideInCassette() {
        _cassettePlayer = FindObjectOfType<CassettePlayer>();

        if (!_cassettePlayer.occupied) {
            _sliding = true;
            Transform _lockedStartPos = _cassettePlayer.LockedStartPosition;
            Transform _target = _cassettePlayer.LockedEndPosition;

            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            _rb.transform.parent = _lockedStartPos.parent;

            transform.position = _lockedStartPos.position;
            _cassettePlayer.occupied = true;
            //_rb.isKinematic = true;
        }

        else {
            transform.parent = _prevParent;
            _onSnapTrigger = false;
            _rb.useGravity = true;
            _rb.drag = 0;        
        }
    }

    /*private void OnTriggerEnter(Collider other) {
        var snapTrigger = other.GetComponent<SnapTrigger>();
        if (snapTrigger != null) {
            _snapTarget = snapTrigger.SnapPosition;
            _onSnapTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        _onSnapTrigger = false;
        _snapTarget = null;
    }*/
}