using UnityEngine;

public class AttackDealer : MonoBehaviour
{
    private int strength = 1; //The strength the hit, should be controlled by another script.
    private float knockbackStr = 10f; //The knockback given, should be controlled by another script.
    private float invincibilityLength = 1.5f;
    private WeaponBase.Affinity typing;

    public void UpdateAttackDealer(WeaponBase weapon)
    {
        strength = weapon.baseStrength;
        knockbackStr = weapon.knockbackStr;
    }
    
    public void SetStrength(int str)
    {
        strength = str;
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
            receiver.RecieveAttack(transform, strength, knockbackStr, invincibilityLength, typing);
        }
    }
}
