using UnityEngine;

public class Cutter : PlayerWeaponBase
{
    public Cutter(OriginalValue<float> speed) : base("Cutter", speed)
    {
        description = "Light: Slash\nHeavy: Range Cutter\nSpecial: Cutter Hell";

        specialCooldown = 15;
        damage = Random.Range(5, 10);

        knockback = new(10, 0);
        hitInterval = 0.3f;
        stunTime = 0.3f;
    }

    public override void LightAttack(Animator anim)
    {
        UpdateValues(7, 1, new(10, 0), 0.1f, 0.2f);
        base.LightAttack(anim);
    }

    public override void HeavyAttack(Animator anim)
    {
        UpdateValues(2, 2, new(7, 0), 0.17f, 0.1f);
        base.HeavyAttack(anim);
    }

    public override void SpecialAttack(Animator anim)
    {
        UpdateValues(1, new(7, 0), 0.17f, 0.2f);
        base.SpecialAttack(anim);
    }
}
