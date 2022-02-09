using UnityEngine;

public class WaterCup : MonoBehaviour
{
    private float _fillAmount;
    public float FillAmount => _fillAmount;

    // private void Start()
    // {
    //     GameManager.instance.taskReferences.waterCups.Add(this);
    // }
    // //
    // private void OnDestroy()
    // {
    //     GameManager.instance.taskReferences.waterCups.Remove(this);
    // }
    //
    // private void OnParticleCollision(GameObject other)
    // {
    //     if (!(_fillAmount >= 50))
    //         _fillAmount++;
    // }
}
