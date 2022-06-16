using UnityEngine;

interface IAttackReceiver
{
    /// <summary>
    /// Receive the attack and its data that is passed through.
    /// </summary>
    /// <param name="attackerPos">The attacker's position.</param>
    /// <param name="strength">The damage dealt.</param>
    /// <param name="knockback">The proposed knockback (Will only be positive)</param>
    /// <param name="stunTime">The stun time to allow the knockback</param>
    public void RecieveAttack(Transform attackerPos, WeaponBase weapon);
}
