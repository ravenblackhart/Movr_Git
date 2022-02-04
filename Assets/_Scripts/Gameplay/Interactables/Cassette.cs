using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Interactions;

public class Cassette : PhysicsObject
{
    //[SerializeField] private _Scripts.Audio.Mixtape Cassette;
    [SerializeField] private _Scripts.Audio.Mixtape Mixtape;
    public AudioClip[] TrackList;
    [SerializeField] private LayerMask _raycastOnLayer;

    public Transform _targetPos;
    public string _musicGenre;

    public bool _sliding;
    public Transform _prevParent;
    CassettePlayer _cassettePlayer;
    public Transform _snapPosition;
    bool triggerFound = false;

    private void Start() {
        TrackList = Mixtape.PlaylistTracks;
        _prevParent = transform.parent;
        _musicGenre = Mixtape.name;
    }

    public override void FixedUpdate() {

        //if (!beingHeld) return;


        if (_onSnapTrigger && !_sliding)
        {
             //Leaving SnapTriggerArea
             if (Vector3.Distance(_holdPos.position, transform.position) > _snapRange)
             {
                 transform.parent = _holdPos;
                 _onSnapTrigger = false;
             }
        }

        base.FixedUpdate();
    }
    
    public void SlideInCassette() {
        _cassettePlayer = FindObjectOfType<CassettePlayer>();

        if (!_cassettePlayer.occupied) {
            _sliding = true;
            _onSnapTrigger = false;
            Transform _lockedStartPos = _cassettePlayer.LockedStartPosition;
            Transform _target = _cassettePlayer.LockedEndPosition;

           
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            
            transform.parent = _lockedStartPos.parent;
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

    IEnumerator TurnOffTrigger() {
        yield return new WaitForSeconds(0.2f);
        if (triggerFound) {
            triggerFound = false;
            _snapPosition.GetComponent<CassettePlayer>()._cassette = null;
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