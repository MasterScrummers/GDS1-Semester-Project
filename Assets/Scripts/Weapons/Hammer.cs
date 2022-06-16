using UnityEngine;

public class Hammer : WeaponBase
{
    public Hammer() : base("Hammer")
    {
        description = "Light: Smash\nHeavy: Spin\nSpecial: Hammer Flip";

        specialCooldown = Random.Range(4, 10);
        int extra = Mathf.RoundToInt(specialCooldown * 0.5f);
        strength = Random.Range(6 + extra, 14 + extra);
    }

    public override void LightAttack(Animator anim)
    {
        //set player speed to 3 (slightly slower)
        UpdateValues(1, new(17, 0), 0.5f, 0.3f);
        base.LightAttack(anim);
    }

    public override void HeavyAttack(Animator anim)
    {
        UpdateValues(2, new(17, 0), 0.3f, 0.3f);
        base.HeavyAttack(anim);
    }

    public override void SpecialAttack(Animator anim)
    {
        //set player speed to 10 (faster) originally 2, but 10 sounds more fun.
        UpdateValues(2, new(10, 0), 0.1f, 0.3f, "HammerSpecial_0");
        base.SpecialAttack(anim);
    }

    public void HammerFlip()
    {
        UpdateValues(5, new(27, 35), 1f, 3f);
    }
}
