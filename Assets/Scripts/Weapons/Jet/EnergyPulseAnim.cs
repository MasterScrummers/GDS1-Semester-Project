using UnityEngine;

public class EnergyPulseAnim : MonoBehaviour
{
    private void DestroySelf()
    {
        Destroy(transform.parent.gameObject);
    }
}
