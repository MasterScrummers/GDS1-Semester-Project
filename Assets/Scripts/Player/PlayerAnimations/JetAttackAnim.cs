using UnityEngine;

public class JetAttackAnim : MonoBehaviour
{
    private Rigidbody2D rb; //The rigidbody of the player

    [SerializeField] private float upStr = 10;
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

            case DashDirection.Upward:
                rb.velocity = transform.up * upStr;
                break;
        }
    }

    private void SpawnEnergyPulse()
    {
        GameObject projectile = poolController.GetObjectFromPool("EnergyPool");
        projectile.transform.position = firePoint.position;
        projectile.transform.eulerAngles = firePoint.parent.transform.eulerAngles;
        projectile.GetComponent<AttackDealer>()?.UpdateAttackDealer(pi.specialWeapon);
    }
}
