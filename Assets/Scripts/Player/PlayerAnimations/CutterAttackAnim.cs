using UnityEngine;

public class CutterAttackAnim : MonoBehaviour
{
    [SerializeField] private PlayerInput pi;
    [SerializeField] private Transform firePoint;
    private PoolController poolController;

    private void Start()
    {
        poolController = DoStatic.GetGameController<PoolController>();
    }

    private GameObject SpawnCutter()
    {
        GameObject projectile = poolController.GetObjectFromPool("CutterPool");
        projectile.transform.position = firePoint.position;
        projectile.transform.eulerAngles = firePoint.parent.transform.eulerAngles;
        projectile.GetComponent<AttackDealer>()?.UpdateAttackDealer(pi.specialWeapon);
        return projectile;
    }
}
