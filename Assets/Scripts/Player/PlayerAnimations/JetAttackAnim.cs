using UnityEngine;

public class JetAttackAnim : MonoBehaviour
{
    [SerializeField] private GameObject energyPulse;
    private Rigidbody2D rb; //The rigidbody of the player

    [SerializeField] private float upStr = 10;
    [SerializeField] private float lightStr = 7;
    [SerializeField] private float heavyStr = 11;

    private enum DashDirection
    {
        Light,
        Heavy,
        Upward,
    }

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void JetDash(DashDirection direction)
    {
        switch (direction)
        {
            case DashDirection.Light:
                rb.velocity = transform.right * lightStr;
                break;

            case DashDirection.Heavy:
                rb.velocity = transform.right * heavyStr;
                break;

            case DashDirection.Upward:
                rb.velocity = transform.up * upStr;
                break;
        }
    }
}
