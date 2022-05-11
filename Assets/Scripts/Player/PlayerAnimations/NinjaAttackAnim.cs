using UnityEngine;

public class NinjaAttackAnim : MonoBehaviour
{
    [SerializeField] private PlayerInput pi;
    [SerializeField] private GameObject kunai;
    [SerializeField] private Transform firePoint;
    private PoolController poolController;

    private void Start()
    {
        poolController = DoStatic.GetGameController<PoolController>();
    }

    private GameObject SpawnKunai()
    {
        GameObject projectile = poolController.GetObjectFromPool("KunaiPool");
        projectile.transform.position = firePoint.position;
        projectile.transform.eulerAngles = firePoint.parent.transform.eulerAngles;
        return projectile;
    }

    private void SpawnLightKunaiProjectile()
    {
        AttackDealer ad = SpawnKunai().GetComponent<AttackDealer>();
        ad.UpdateAttackDealer(pi.lightWeapon);
    }

    private void SpawnHeavyKunaiProjectile()
    {
        for(int i = 0; i < 10; i++)
        {
            GameObject projectile = SpawnKunai();
            Vector3 rot = projectile.transform.eulerAngles;
            rot.z = Random.Range(-45, 45);
            projectile.transform.eulerAngles = rot;
            AttackDealer ad = projectile.GetComponent<AttackDealer>();
            ad.UpdateAttackDealer(pi.heavyWeapon);
        }
    }

    private void SpawnSpecialKunaiProject()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject projectile = SpawnKunai();
        }
    }
}
