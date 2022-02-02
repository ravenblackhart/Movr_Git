using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CassettePlayer : SnapTrigger
{
    public bool occupied = false;
    private Cassette _cassette;
    public Transform cassetteInPlayer;

    public Transform LockedStartPosition;
    public Transform LockedEndPosition;

    [SerializeField] private Button _button;

    private void Awake() {
        _button.onInteractEvent.AddListener(Eject);
    }

    private void OnTriggerEnter(Collider other) {
        print("Did trigger");
        if (other.TryGetComponent(out Cassette cassette)) {
            print("Found Cassette");
            _cassette = cassette;
            _cassette.OnSnapTrigger = true;
            _cassette.transform.rotation = transform.rotation;

            //transform the position
            //change parent
            print(_cassette.name);
        }
    }

    private void FixedUpdate() {

        if (_cassette.OnSnapTrigger) {
            _cassette.transform.parent = _cassette._prevParent;
            //transform.position = _snapTarget.position;
            transform.rotation = transform.rotation;
            Vector3 moveDirection = (transform.position - _cassette.transform.position);
            _cassette._rb.AddForce(moveDirection * _cassette._snapSpeed);        
        }

        if (_cassette._sliding) {
            //_cassette.transform.parent = _cassette._prevParent;
            _cassette.OnSnapTrigger = false;
            Vector3 moveDirection = (LockedEndPosition.position - _cassette.transform.position);
            _cassette._rb.AddForce(moveDirection * _cassette._snapSpeed);

            if (Vector3.Distance(_cassette.transform.position, LockedEndPosition.transform.position) <= 0.01f) {
                cassetteInPlayer = _cassette.transform;
                _cassette = null;
                //_rb.isKinematic = true;
            }
        }
    }

    private void Eject() {
        if (cassetteInPlayer != null) {
            Rigidbody _cassetteRb = cassetteInPlayer.GetComponent<Rigidbody>();
            Cassette _cassetteScript = cassetteInPlayer.GetComponent<Cassette>();

            cassetteInPlayer.parent = _cassetteScript._prevParent;
            _cassetteScript._sliding = false;
            _cassetteRb.isKinematic = false;
            _cassetteRb.useGravity = true;
            _cassetteRb.drag = 0;
            _cassetteRb.AddForce(-transform.forward * 100 + transform.right * 50);
            occupied = false;
            StartCoroutine(TurnOnCollision());
        }

        else {
            //explode, killing thousands
        }
    }

    IEnumerator TurnOnCollision() {
        yield return new WaitForSeconds(0.1f);
        cassetteInPlayer.GetComponent<BoxCollider>().isTrigger = false;
        cassetteInPlayer = null;
    }

    private void OnDisable() {
        _button.onInteractEvent.RemoveListener(Eject);
    }
}
