using UnityEngine;

public class Jet : PlayerWeaponBase
{
    public Jet(OriginalValue<float> speed) : base("Jet", speed)
    {
        description = "Light: Dash\nHeavy: Long Dash\n Special: Dash with energy pulse";

        specialCooldown = Random.Range(2, 3);
        int extra = Mathf.RoundToInt(specialCooldown * 0.5f);
        damage = Random.Range(5 + extra, 7 + extra);
    }

    public override void LightAttack(Animator anim)
    {
        UpdateValues(1, new(20, 0), 0.3f, 0.2f);
        base.LightAttack(anim);
    }

    public override void HeavyAttack(Animator anim)
    {
        UpdateValues(3, 2, new(40, 0), 0.3f, 0.2f);
        base.HeavyAttack(anim);
    }

    public override void SpecialAttack(Animator anim)
    {
        UpdateValues(3, new(50, 0), 0.5f, 1f);
        base.SpecialAttack(anim);
    }
}