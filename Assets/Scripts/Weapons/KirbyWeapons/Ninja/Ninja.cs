using UnityEngine;

public class Ninja : PlayerWeaponBase
{
    public Ninja(OriginalValue<float> speed) : base("Ninja", speed)
    {
        description = "Light: Kunai\nHeavy: Shotgun like Kunai\n Special: Kunai bullet hell";
        
        specialCooldown = 10;
        damage = Random.Range(3 , 5);

        knockback = new(3, 0);
        hitInterval = 1f;
        stunTime = 0f;
    }

    public override void LightAttack(Animator anim)
    {
        UpdateValues(1, knockback, hitInterval, stunTime, "KunaiLight");
        base.LightAttack(anim);
    }

    public override void HeavyAttack(Animator anim)
    {
        UpdateValues(1, knockback, hitInterval, stunTime);
        base.HeavyAttack(anim);
    }

    public override void SpecialAttack(Animator anim)
    {
        UpdateValues(1, knockback, hitInterval, stunTime);
        base.SpecialAttack(anim);
    }
}
