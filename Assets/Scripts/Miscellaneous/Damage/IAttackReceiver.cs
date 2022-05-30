using UnityEngine;

interface IAttackReceiver
{
    public void RecieveAttack(Transform attackerPos, int strength, Vector2 knockback, float invincibilityTime, float stunTime);
}
