using UnityEngine;

public class CustomerThirst : MonoBehaviour
{
    private float _fillAmount;
    public float FillAmount => _fillAmount;

    float timeSinceWater;
    
    private void Start()
    {
        _fillAmount = 0;
    }

    private void Update()
    {
        if (timeSinceWater <= 0)
        {
            _fillAmount = Mathf.Max(0f, _fillAmount - Time.deltaTime * 30f);
        }

        timeSinceWater -= Time.deltaTime;
    }

    private void OnParticleCollision(GameObject other)
    {
        _fillAmount = Mathf.Min(_fillAmount + 1f, 40f);

        timeSinceWater = 0.5f;
    }
}
