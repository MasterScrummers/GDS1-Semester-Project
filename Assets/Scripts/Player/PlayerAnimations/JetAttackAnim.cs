using UnityEngine;

public class JetAttackAnim : MonoBehaviour
{
    private Rigidbody2D rb; //The rigidbody of the player

    [SerializeField] private float lightStr = 7;
    [SerializeField] private float heavyStr = 11;
    [SerializeField] private float SpecialStr = 20;

    [SerializeField] private Transform firePoint;
    private PoolController poolController;
    private JumpComponent jump;

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
        poolController = DoStatic.GetGameController<PoolController>();
        jump = DoStatic.GetPlayer<JumpComponent>();
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

            case DashDirection.Special:
                rb.velocity = transform.right * SpecialStr;
                break;

            case DashDirection.backward:
                rb.velocity = transform.right * -lightStr;
                break;

        }
    }

    private void LockYConstraints(bool doLock)
    {
        jump.fallGravity.value = doLock ? 0 : jump.fallGravity.originalValue;
    }

    private void SetisSliding(string state)
    {
        //pi.isSliding = state.Equals("True");
    }
}
