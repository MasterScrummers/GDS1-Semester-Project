using UnityEngine;

public class AttackDealer : MonoBehaviour
{
    [SerializeField] protected int strength = 1; //The strength the hit, should be controlled by another script.
    protected int strengthMult = 1;

    [SerializeField] protected float knockbackStr = 1f; //The knockback given, should be controlled by another script.
    protected float knockbackStrMult = 1;

    [SerializeField] protected float invincibilityLength = 1.5f;
    [SerializeField] protected float invincibilityTime;
    [SerializeField] protected WeaponBase.Affinity typing;
    [SerializeField] protected float stunTime;

    public void UpdateAttackDealer(WeaponBase weapon)
    {
        strength = weapon.baseStrength;
        knockbackStr = weapon.knockbackStr;
        stunTime = weapon.stunTime;
        typing = weapon.weaponType;
    }
    
    public void SetStrengthMult(int mult)
    {
        strengthMult = mult;
    }

    public void SetknockbackStrMult(float mult)
    {
        knockbackStrMult = mult;
    }

    public void SetInvincibilityLength(float length)
    {
        invincibilityLength = length;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        IAttackReceiver receiver = collision.GetComponent<IAttackReceiver>();
        
        if (receiver != null)
        {
            receiver.RecieveAttack(transform, strength * strengthMult, knockbackStr * knockbackStrMult, invincibilityLength, stunTime, typing);
        }
    }
}
