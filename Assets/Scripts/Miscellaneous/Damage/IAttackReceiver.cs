using UnityEngine;

interface IAttackReceiver
{
    /// <summary>
    /// Receive the attack and its data that is passed through.
    /// </summary>
    /// <param name="attackerPos">The attacker's position.</param>
    public void RecieveAttack(Transform attackerPos, WeaponBase weapon);
}
