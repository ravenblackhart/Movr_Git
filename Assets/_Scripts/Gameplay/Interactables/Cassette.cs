using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cassette : PhysicsObject
{
    public _Scripts.Audio.Mixtape Mixtape;

    public Transform _targetPos;


    public void SlideInCassette() {
        if (!_snapTrigger.occupied) {
            _sliding = true;
            Transform _lockedStartPos = GameObject.Find("CassetteTrigger").GetComponent<SnapTrigger>().LockedStartPosition;
            _target = GameObject.Find("CassetteTrigger").GetComponent<SnapTrigger>().LockedEndPosition;

            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            _rb.transform.parent = _lockedStartPos.parent;

            transform.position = _lockedStartPos.position;
            _snapTrigger.occupied = true;
            //_rb.isKinematic = true;
        }

        else {
            _rb.useGravity = true;
            _rb.drag = 0;                               
        }
    }

    private void OnTriggerEnter(Collider other) {
        var snapTrigger = other.GetComponent<SnapTrigger>();
        if (snapTrigger != null) {
            _snapTarget = snapTrigger.SnapPosition;
            _onSnapTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        _onSnapTrigger = false;
        _snapTarget = null;
    }
}