using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnackBar :  MonoBehaviour
{
    public bool _touchingCustomer = false;
    AudioManager _audioManager;

    [SerializeField] GameObject crumbs;

    float snackTimer = 0;

    private void Awake() {
        _audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update() {
       
    }

    private void OnCollisionEnter(Collision col) {
        if (col.transform.name == "Customer(Clone)") {
            StartCoroutine(EatSnackbar());
        }
    }


    IEnumerator EatSnackbar() {
        _touchingCustomer = true;   
        //Play chomp audio?
        _audioManager.Play("Chomp");
        //Maybe call dialogue saying I'm not hungry?
        yield return null;
        Instantiate(crumbs, transform.position, Quaternion.identity);
        yield return null;
        _touchingCustomer = false;
        Destroy(gameObject);
    }
}
