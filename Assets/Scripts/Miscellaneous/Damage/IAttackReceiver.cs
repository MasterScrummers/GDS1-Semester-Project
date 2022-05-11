using UnityEngine;

interface IAttackReceiver
{
    public void RecieveAttack(Transform attackerPos, int strength, float knockbackStr, float invincibilityTime, float stunTime, WeaponBase.Affinity typing);
}
