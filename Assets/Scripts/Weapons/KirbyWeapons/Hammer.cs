using UnityEngine;

public class Hammer : PlayerWeaponBase
{
    public Hammer(OriginalValue<float> speed) : base("Hammer", speed)
    {
        description = "Light: Smash\nHeavy: Spin\nSpecial: Hammer Flip";

        specialCooldown = 12;
        damage = Random.Range(12, 18);
    }

    public override void LightAttack(Animator anim)
    {
        UpdateValues(3, 1, new(18, 0), 0.5f, 0.8f, "HammerLight1");
        base.LightAttack(anim);
    }

    public override void HeavyAttack(Animator anim)
    {
        UpdateValues(1, new(15, 0), 0.5f, 0.7f, "HammerHeavy");
        base.HeavyAttack(anim);
    }

    public override void SpecialAttack(Animator anim)
    {
        UpdateValues(7, 1, new(10, 0), 0.3f, 0.5f, "HammerSpecial1"); //Speed originally 2
        base.SpecialAttack(anim);
    }

    public void HammerFlip()
    {
        UpdateValues(4, new(27, 35), 1f, 3f);
    }
}
