using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnackBar : PhysicsObject
{
    [SerializeField] GameObject crumbs;

    private void Start() {
        touchCustomerUnityEvent.AddListener(EatSnackbar);
    }

    private void EatSnackbar() {
        if (GameManager.instance.currentTaskType != TaskType.GiveSnackBar)
            return;

        GameManager.instance.audioManager.Play("Chomp");

        Instantiate(crumbs, transform.position, Quaternion.identity);

        // SetActive because we'll be using pooling later so we don't wanna destroy anything
        gameObject.SetActive(false);
    }
}
