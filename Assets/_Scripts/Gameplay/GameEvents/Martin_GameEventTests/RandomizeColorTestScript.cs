using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomizeColorTestScript : MonoBehaviour
{
    private Renderer _renderer;
    private bool atTop = false;
    private Vector3 _origin;
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        _origin = transform.position;
    }

    public void SetRandomColor()
    {
        Color color = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f));

        _renderer.material.color = color;
    }

    public void MovePosition()
    {
        SetRandomColor();
        if (!atTop)
        {
            Vector3 offset = new Vector3(0, .5f, 0);
            transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z) + offset;
        }
        else
        {
            transform.position = _origin;
        }
        atTop = !atTop;
    }
}
