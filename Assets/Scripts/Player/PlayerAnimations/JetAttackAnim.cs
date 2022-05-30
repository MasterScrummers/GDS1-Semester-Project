using UnityEngine;

public class JetAttackAnim : MonoBehaviour
{
    private Rigidbody2D rb; //The rigidbody of the player

    [SerializeField] private float lightStr = 7;
    [SerializeField] private float heavyStr = 11;
    [SerializeField] private Transform firePoint;
    private PoolController poolController;

    private enum DashDirection
    {
        Light,
        Heavy,
        Upward,
    }

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        poolController = DoStatic.GetGameController<PoolController>();
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

        }
    }

    private void LockYConstraints(string state)
    {
        rb.constraints = state.Equals("Lock") ? RigidbodyConstraints2D.FreezePositionY : RigidbodyConstraints2D.None;
    }
}
