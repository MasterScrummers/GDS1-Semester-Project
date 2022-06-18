using UnityEngine;

public class EnemyWeaponBase : WeaponBase
{
    public EnemyWeaponBase() {}

    protected void UpdateValues(Vector2 knockback, float hitInterval, string sfx = "")
    {
        this.knockback = knockback;
        this.hitInterval = hitInterval;
        this.sfx = sfx;
    }
}
