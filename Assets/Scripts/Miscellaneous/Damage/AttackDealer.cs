using System.Collections.Generic;
using UnityEngine;

public class AttackDealer : MonoBehaviour
{
    [Header("Attack Dealer Parameters")]
    [SerializeField] protected int strength = 1; //The strength the hit, should be controlled by another script.
    protected int strengthMult = 1;

    [SerializeField] protected Vector2 knockback = Vector2.one; //The knockback to give.
    [SerializeField] protected float invincibilityLength = 1.5f;
    [SerializeField] protected float stunTime;

    private readonly Dictionary<Collider2D, IAttackReceiver> victims = new();

    public void UpdateAttackDealer(WeaponBase weapon)
    {
        strength = weapon.baseStrength;
        knockback = weapon.knockbackStr;
        invincibilityLength = weapon.invincibilityTime;
        stunTime = weapon.stunTime;
    }
    
    public void SetStrengthMult(int mult)
    {
        strengthMult = mult;
    }

    public void SetKnockbackX(float x)
    {
        knockback.x = x;
    }

    public void SetKnockbackY(float y)
    {
        knockback.y = y;
    }

    public void SetInvincibilityLength(float length)
    {
        invincibilityLength = length;
    }

    public void SetStunTime(float time)
    {
        stunTime = time;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            return;
        }

        if (collision.TryGetComponent<IAttackReceiver>(out var receiver))
        {
            victims.Add(collision, receiver);
            OnTriggerStay2D(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (victims.ContainsKey(collision))
        {
            victims[collision].RecieveAttack(transform, strength * strengthMult, knockback, invincibilityLength, stunTime);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (victims.ContainsKey(other))
        {
            victims.Remove(other);
        }
    }
}
