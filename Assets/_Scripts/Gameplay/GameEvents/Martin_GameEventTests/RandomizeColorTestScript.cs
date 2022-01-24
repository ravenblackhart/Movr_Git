using UnityEngine;
using Random = UnityEngine.Random;

public class RandomizeColorTestScript : MonoBehaviour
{
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void SetRandomColor()
    {
        Color color = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f));

        _renderer.material.color = color;
    }
}
