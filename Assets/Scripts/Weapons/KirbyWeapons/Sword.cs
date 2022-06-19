using UnityEngine;

public class Sword : PlayerWeaponBase
{
    public Sword(OriginalValue<float> speed) : base("Sword", speed)
    {
        description = "Light: Slash\nHeavy: Stab\n Special: Spin";

        specialCooldown = Random.Range(5, 9);
        int extra = Mathf.RoundToInt(specialCooldown * 0.5f);
        damage = Random.Range(5 + extra, 8 + extra);
    }

    public override void LightAttack(Animator anim)
    {
        UpdateValues(1, new(15, 0), 0.2f, 0.25f);
        base.LightAttack(anim);
    }

    public override void HeavyAttack(Animator anim)
    {
        UpdateValues(3, 2, new(20, 0), 0.3f, 0.25f);
        base.HeavyAttack(anim);
    }

    public override void SpecialAttack(Animator anim)
    {
        UpdateValues(15, 1, new(-15, -15), 0.1f, 0.5f);
        base.SpecialAttack(anim);
    }
}
