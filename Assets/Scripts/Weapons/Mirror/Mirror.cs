using UnityEngine;

public class Mirror : PlayerWeaponBase
{
    public Mirror(OriginalValue<float> speed) : base("Mirror", speed)
    {
        description = "Light: Short Invincibility + Pushback\nHeavy: Summon 4 small mirror\nSpecial: Mirror Bomb";

        specialCooldown = Random.Range(5, 7);
        strength = Random.Range(2, 5);

        stunTime = 1;
    }

    public override void LightAttack(Animator anim)
    {
        UpdateValues(3, 1, new(20, 0), 0.5f, stunTime);
        base.LightAttack(anim);
    }

    public override void HeavyAttack(Animator anim) //Look into how this works
    {
        UpdateValues(1, new(10, 0), 0.5f, stunTime);
        base.HeavyAttack(anim);
    }

    public override void SpecialAttack(Animator anim) //Look into how this works
    {
        UpdateValues(1, new(0, 0), 0.1f, stunTime);
        base.SpecialAttack(anim);
    }
}
