using UnityEngine;

public class CustomerThirst : MonoBehaviour
{
    private int _fillAmount;
    public float FillAmount => _fillAmount;
    
    private void Start()
    {
        _fillAmount = 0;
    }
    
    private void OnParticleCollision(GameObject other)
    {
        if (!(_fillAmount >= 110))
            _fillAmount++;
    }
}
