using UnityEngine;

public class Ninja : PlayerWeaponBase
{
    public Ninja(OriginalValue<float> speed) : base("Ninja", speed)
    {
        description = "Light: Kunai\nHeavy: Shotgun like Kunai\n Special: Kunai bullet hell";
        
        specialCooldown = Random.Range(5, 10);
        damage = 1;

        knockback = new(1, 0);
        hitInterval = 1f;
        stunTime = 1f;
    }

    public override void LightAttack(Animator anim)
    {
        UpdateValues(1, knockback, hitInterval, stunTime);
        base.LightAttack(anim);
    }

    public override void HeavyAttack(Animator anim)
    {
        UpdateValues(2, knockback, hitInterval, stunTime);
        base.HeavyAttack(anim);
    }

    public override void SpecialAttack(Animator anim)
    {
        UpdateValues(1, knockback, hitInterval, stunTime);
        base.SpecialAttack(anim);
    }
}