using UnityEngine;

public class AttackDealer : MonoBehaviour
{
    private int strength = 1; //The strength the hit, should be controlled by another script.
    private int strengthMult = 1;
    private float knockbackStr = 1f; //The knockback given, should be controlled by another script.
    private float invincibilityLength = 1.5f;
    private WeaponBase.Affinity typing;

    public void UpdateAttackDealer(WeaponBase weapon)
    {
        strength = weapon.baseStrength;
        knockbackStr = weapon.knockbackStr;
        typing = weapon.weaponType;
    }
    
    public void SetStrengthMult(int mult)
    {
        strengthMult = mult;
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
            receiver.RecieveAttack(transform, strength * strengthMult, knockbackStr, invincibilityLength, typing);
        }
    }
}
