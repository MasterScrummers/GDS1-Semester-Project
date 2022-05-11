using UnityEngine;

public class NinjaAttackAnim : MonoBehaviour
{
    [SerializeField] private PlayerInput pi;
    private PlayerInvincibility invincibility;
    [SerializeField] private Transform firePoint;
    private PoolController poolController;
    private float angle;

    private void Start()
    {
        invincibility = GetComponent<PlayerInvincibility>();
        poolController = DoStatic.GetGameController<PoolController>();
        angle = 0f;
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
        SpawnKunai().GetComponent<AttackDealer>()?.UpdateAttackDealer(pi.lightWeapon);
    }

    private void SpawnHeavyKunaiProjectile()
    {
        for(int i = 0; i < 10; i++)
        {
            GameObject projectile = SpawnKunai();
            Vector3 rot = projectile.transform.eulerAngles;
            rot.z = Random.Range(-45, 45);
            projectile.transform.eulerAngles = rot;
            projectile.GetComponent<AttackDealer>()?.UpdateAttackDealer(pi.heavyWeapon);
        }
    }

    private void SpawnSpecialKunaiProjectile(string state)
    {
        switch (state)
        {
            case "On":
                InvokeRepeating("BulletHell", 0f, 0.01f);
                invincibility.StartInvincible(1);
                break;

            case "Off":
                CancelInvoke();
                break;
        }

    }

    private void BulletHell()
    {
        GameObject projectile = poolController.GetObjectFromPool("KunaiPool");
        projectile.transform.position = transform.position;
        Vector3 rot = projectile.transform.eulerAngles;
        rot.z = angle;
        projectile.transform.eulerAngles = rot;
        projectile.GetComponent<AttackDealer>()?.UpdateAttackDealer(pi.specialWeapon);
        angle += 20f;
    }
}
