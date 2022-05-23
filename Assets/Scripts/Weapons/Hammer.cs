using UnityEngine;

public class Hammer : WeaponBase
{
    public Hammer() : base()
    {
        weaponName = "Hammer";
        description = "Light: Smash\nHeavy: Spin\nSpecial: Hammer Flip";
        animPath = basePath + weaponName + "."; //Don't forget the dot!

        specialCooldown = Random.Range(4, 12);
        int extra = Mathf.RoundToInt(specialCooldown * 0.5f);
        baseStrength = Random.Range(3 + extra, 12 + extra);
        knockbackStr = new(17f, 0);
        invincibilityTime = 0.3f;
    }

    public override void LightAttack(Animator anim)
    {
        anim.Play(animPath + "HammerLight");
    }

    public override void HeavyAttack(Animator anim)
    {
        anim.Play(animPath + "HammerHeavy");
    }

    public override void SpecialAttack(Animator anim)
    {
        anim.Play(animPath + "HammerSpecial");
    }
}
