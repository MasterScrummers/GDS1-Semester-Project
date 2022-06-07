using System.Collections.Generic;
using UnityEngine;

public class AttackDealer : MonoBehaviour
{
    [SerializeField] protected int strength = 1; //The strength the hit, should be controlled by another script.
    protected int strengthMult = 1;

    [SerializeField] protected Vector2 knockback = Vector2.one; //The knockback given, should be controlled by another script.

    [SerializeField] protected float invincibilityLength = 1.5f;
    [SerializeField] protected float invincibilityTime;
    [SerializeField] protected float stunTime;

    private Dictionary<Collider2D, IAttackReceiver> victims = new();

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
        IAttackReceiver receiver = collision.GetComponent<IAttackReceiver>();
        if (receiver != null)
        {
            victims.Add(collision, receiver);
            OnTriggerStay2D(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (victims.ContainsKey(collision) && victims[collision] != null)
        {
            victims[collision].RecieveAttack(transform, strength * strengthMult, knockback, invincibilityLength, stunTime);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        victims.Remove(other);
    }
}
