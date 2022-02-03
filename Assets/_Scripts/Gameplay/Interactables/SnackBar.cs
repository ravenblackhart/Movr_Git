using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnackBar : PhysicsObject
{
    [SerializeField] GameObject crumbs;

    private void Start() {
        GameManager.instance.taskReferences.snackBars.Add(this);
        touchCustomerUnityEvent.AddListener(EatSnackbar);
    }

    private void EatSnackbar() {
        if (GameManager.instance.currentTaskType != TaskType.GiveSnackBar)
            return;
    
        StartCoroutine(ShittyFix());
    }

    IEnumerator ShittyFix() {
        yield return null;
        Instantiate(crumbs, transform.position, Quaternion.identity);
        GameManager.instance.audioManager.Play("Chomp");
        // SetActive because we'll be using pooling later so we don't wanna destroy anything
        gameObject.SetActive(false);

    }
}
