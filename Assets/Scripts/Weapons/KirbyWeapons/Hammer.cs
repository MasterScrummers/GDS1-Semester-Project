using UnityEngine;

public class Hammer : PlayerWeaponBase
{
    public Hammer(OriginalValue<float> speed) : base("Hammer", speed)
    {
        description = "Light: Smash\nHeavy: Spin\nSpecial: Hammer Flip";

        specialCooldown = Random.Range(4, 10);
        int extra = Mathf.RoundToInt(specialCooldown * 0.5f);
        damage = Random.Range(6 + extra, 14 + extra);
    }

    public override void LightAttack(Animator anim)
    {
        UpdateValues(3, 1, new(17, 0), 0.5f, 0.3f);
        base.LightAttack(anim);
    }

    public override void HeavyAttack(Animator anim)
    {
        UpdateValues(2, new(17, 0), 0.3f, 0.3f);
        base.HeavyAttack(anim);
    }

    public override void SpecialAttack(Animator anim)
    {
        UpdateValues(10, 2, new(10, 0), 0.1f, 0.3f, "HammerSpecial_0"); //Speed originally 2
        base.SpecialAttack(anim);
    }

    public void HammerFlip()
    {
        UpdateValues(5, new(27, 35), 1f, 3f);
    }
}
