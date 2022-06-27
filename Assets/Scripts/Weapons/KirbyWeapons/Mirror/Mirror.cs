using UnityEngine;

public class Mirror : PlayerWeaponBase
{
    public Mirror(OriginalValue<float> speed) : base("Mirror", speed)
    {
        description = "Light: Short Invincibility + Pushback\nHeavy: Summon 4 small mirror\nSpecial: Mirror Bomb";

        specialCooldown = 7;
        damage = Random.Range(2, 4);

        stunTime = 1;
    }

    public override void LightAttack(Animator anim)
    {
        UpdateValues(3, 0, new(20, 0), 1f, 1f);
        base.LightAttack(anim);
    }

    public override void HeavyAttack(Animator anim) //Look into how this works
    {
        UpdateValues(2, new(7, 0), 0.3f, 0.2f);
        base.HeavyAttack(anim);
    }

    public override void SpecialAttack(Animator anim) //Look into how this works
    {
        UpdateValues(3, new(25, 0), 0.5f, 1f);
        base.SpecialAttack(anim);
    }
}
