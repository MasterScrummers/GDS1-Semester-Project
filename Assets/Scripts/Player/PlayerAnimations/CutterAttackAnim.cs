using UnityEngine;

public class CutterAttackAnim : MonoBehaviour
{
    [SerializeField] private PlayerInput pi;
    private PoolController poolController;
    [SerializeField] GameObject cutterPivot;
    private bool hasSecondWave = false;

    private void Start()
    {
        poolController = DoStatic.GetGameController<PoolController>();
    }

    private void Update()
    {
        if (hasSecondWave && DoStatic.GetChildren(cutterPivot.transform).Length == 0)
        {
            hasSecondWave = false;
            CutterSpecial2();
        }
    }

    /// <summary>
    /// For animation
    /// </summary>
    private void SpawnCutter()
    {
        GameObject projectile = poolController.GetObjectFromPool("CutterPool");
        projectile.transform.position = cutterPivot.transform.position;
        projectile.transform.eulerAngles = pi.transform.eulerAngles.y < 90 ? new Vector3(0, 0, 0) : new Vector3(0, 0, 180);
        projectile.GetComponent<AttackDealer>()?.UpdateAttackDealer(pi.heavyWeapon);
    }

    private GameObject SpawnSpecialCutter(Vector3 angle)
    {
        GameObject projectile = poolController.GetObjectFromPool("CutterPool");
        projectile.transform.position = cutterPivot.transform.position;
        projectile.transform.eulerAngles = angle;
        projectile.GetComponent<AttackDealer>()?.UpdateAttackDealer(pi.specialWeapon);
        return projectile;
    }

    private void CutterSpecial()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject projectile = SpawnSpecialCutter(new Vector3(cutterPivot.transform.eulerAngles.x, cutterPivot.transform.eulerAngles.y, 90f * i));
            projectile.transform.parent = cutterPivot.transform;
        }
        hasSecondWave = true;
    }

    private void CutterSpecial2()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject projectile = SpawnSpecialCutter(new Vector3(cutterPivot.transform.eulerAngles.x, cutterPivot.transform.eulerAngles.y, 45 * i));
            projectile.transform.parent = cutterPivot.transform;
        }
    }
}
