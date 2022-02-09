using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindshieldController : MonoBehaviour
{
    bool wiping;

    float dirtyness;
    float lastDirtyness = -1f;

    Material windshieldMaterial;

    [SerializeField]
    Transform wiper;

    [SerializeField]
    MeshRenderer windshieldRenderer;

    // Start
    void Start()
    {
        windshieldMaterial = Instantiate(windshieldRenderer.sharedMaterial);

        windshieldRenderer.sharedMaterial = windshieldMaterial;

        StartCoroutine(DirtyWindshield());
    }

    // Update
    void Update()
    {
        if (dirtyness != lastDirtyness)
        {
            Debug.Log("Windshield Dirtied");

            windshieldMaterial.SetFloat("_Dirtyness", dirtyness);

            lastDirtyness = dirtyness;
        }
    }

    public void TryStartWipers()
    {
        if (wiping)
            return;

        StartCoroutine(RunWipers());
    }

    IEnumerator RunWipers()
    {
        wiping = true;

        AnimationCurve animationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        for (float f = 0f; f < 5f; f = Mathf.Min(f + Time.deltaTime, 5f))
        {
            float progress = Mathf.Sin((f - 0.5f) * Mathf.PI);

            if (f < 1f)
            {
                progress = animationCurve.Evaluate(f);
            }
            else if (f > 4f)
            {
                progress = -animationCurve.Evaluate(5f - f);
            }

            dirtyness = Mathf.Clamp01(dirtyness - Time.deltaTime / 3f);

            wiper.localEulerAngles = Vector3.up * progress * 30f;

            yield return null;
        }

        wiper.localEulerAngles = Vector3.zero;

        wiping = false;
    }

    IEnumerator DirtyWindshield()
    {
        yield return new WaitForSeconds(Random.Range(60f, 75f));

        dirtyness = 1f;

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(45f, 75f));

            dirtyness = 1f;
        }
    }
}
