using UnityEngine;

public class Jet : PlayerWeaponBase
{
    public Jet(OriginalValue<float> speed) : base("Jet", speed)
    {
        description = "Light: Dash\nHeavy: Long Dash\n Special: Explosive Dash";

        specialCooldown = 8;
        damage = Random.Range(10, 13);
    }

    public override void LightAttack(Animator anim)
    {
        UpdateValues(1, new(18, 0), 1.5f, 0.8f, "JetLight");
        base.LightAttack(anim);
    }

    public override void HeavyAttack(Animator anim)
    {
        UpdateValues(3, 2, new(21, 0), 1.5f, 0.8f, "JetHeavy");
        base.HeavyAttack(anim);
    }

    public override void SpecialAttack(Animator anim)
    {
        UpdateValues(3, new(30, 0), 0.3f, 1.5f, "JetSpecial");
        base.SpecialAttack(anim);
    }
}
