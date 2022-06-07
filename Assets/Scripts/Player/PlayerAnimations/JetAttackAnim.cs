using UnityEngine;

public class JetAttackAnim : MonoBehaviour
{
    private Rigidbody2D rb; //The rigidbody of the player
    private PlayerInput pi;

    [SerializeField] private float backSpd = 5;
    [SerializeField] private float lightSpd = 7;
    [SerializeField] private float heavySpd = 11;
    [SerializeField] private float specialSpd = 20;

    private enum DashDirection
    {
        Light,
        Heavy,
        Special,
        backward,
    }

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        pi = rb.GetComponent<PlayerInput>();
    }

    private void JetDash(DashDirection direction)
    {
        rb.velocity = transform.right * direction switch
        {
            DashDirection.Light => lightSpd,
            DashDirection.Heavy => heavySpd,
            DashDirection.Special => specialSpd,
            DashDirection.backward => -backSpd,
            _ => 0
        };
    }

    private void SetSliding(int slidingState)
    {
        pi.isSliding = slidingState == 1;
    }
}
