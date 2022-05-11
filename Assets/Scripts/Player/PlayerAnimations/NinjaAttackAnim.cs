using UnityEngine;

public class NinjaAttackAnim : MonoBehaviour
{
    [SerializeField] private GameObject kunai;
    [SerializeField] private Transform firePoint;
    private PoolController poolController;
    private float angle;

    private void Start()
    {
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
        SpawnKunai();
    }

    private void SpawnHeavyKunaiProjectile()
    {
        for(int i = 0; i < 10; i++)
        {
            GameObject projectile = SpawnKunai();
            Vector3 rot = projectile.transform.eulerAngles;
            rot.z = Random.Range(-45, 45);
            projectile.transform.eulerAngles = rot;
        }
    }

    private void SpawnSpecialKunaiProjectile(string state)
    {
        switch (state)
        {
            case "On":
                InvokeRepeating("BulletHell", 0f, 0.01f);
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

        angle += 20f;
    }
}
