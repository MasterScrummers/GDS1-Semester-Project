using UnityEngine;

public class CutterAttackAnim : MonoBehaviour
{
    [SerializeField] private PlayerInput pi;
    private PoolController poolController;
    [SerializeField] GameObject cutterPivot;

    private void Start()
    {
        poolController = DoStatic.GetGameController<PoolController>();
    }

    private GameObject SpawnSpecialCutter(Vector3 angle)
    {
        GameObject projectile = poolController.GetObjectFromPool("CutterPool");
        projectile.transform.position = cutterPivot.transform.position;
        projectile.transform.eulerAngles = angle;
        projectile.GetComponent<AttackDealer>()?.UpdateAttackDealer(pi.specialWeapon);
        return projectile;
    }

    private GameObject SpawnCutter()
    {
        GameObject projectile = poolController.GetObjectFromPool("CutterPool");
        projectile.transform.position = cutterPivot.transform.position;
        projectile.transform.eulerAngles = cutterPivot.transform.parent.transform.eulerAngles;
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
    }

    private void CutterSpecial2()
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject projectile = SpawnSpecialCutter(new Vector3(cutterPivot.transform.eulerAngles.x, cutterPivot.transform.eulerAngles.y, 45 * i));
            projectile.transform.parent = cutterPivot.transform;
        }
    }
}
