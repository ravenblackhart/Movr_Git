using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnackBar :  MonoBehaviour
{
    public bool _touchingCustomer = false;
    float snackTimer = 0;

    private void Update() {
        /*if (_touchingCustomer) {
            snackTimer += Time.deltaTime;
        }

        else {
            snackTimer = 0;
        }

        if (snackTimer >= 2) {
            Destroy(gameObject);
        }*/
    }

    private void OnCollisionEnter(Collision col) {
        if (col.transform.name == "Customer(Clone)") {
            StartCoroutine(EatSnackbar());
        }
    }


    IEnumerator EatSnackbar() {
        _touchingCustomer = true;
        //Play chomp audio?
        //Maybe call dialogue saying I'm not hungry?
        yield return new WaitForSeconds(0.5f);
        _touchingCustomer = false;
        Destroy(gameObject);
    }
}
