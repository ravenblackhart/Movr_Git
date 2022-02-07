using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    [SerializeField] private float _closeTime;
    [SerializeField] private float _closeSpeed;
    private float _timer;

    private Lever _lever;

    void Awake()
    {
        _lever = GetComponent<Lever>();
    }

    void Update()
    {
        if (_lever.LeverValue > 0) {
            _timer += Time.deltaTime;

            if (_lever.OnDrag) {
                _timer = 0;
            }

            if (_timer >= _closeTime) {
                _lever.MoveDirection = _closeSpeed * -1f;
            }
        }

        else {
            _timer = 0;
        }
    }
}
