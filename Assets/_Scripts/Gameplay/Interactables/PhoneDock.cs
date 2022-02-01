using UnityEngine;

public class PhoneDock : SnapTrigger
{
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Phone phone))
        {
            phone.ChargeAmount += Time.deltaTime;
        }
    }
}