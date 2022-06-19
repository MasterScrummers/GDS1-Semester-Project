using UnityEngine;

public class EnemyWeaponBase : WeaponBase
{
    public EnemyWeaponBase(int damage = 1) {
        this.damage = damage;
    }

    protected void UpdateValues(Vector2 knockback, float hitInterval, string sfx = "")
    {
        this.knockback = knockback;
        this.hitInterval = hitInterval;
        this.sfx = sfx;
    }
}
