using UnityEngine;

public class Mirror : WeaponBase
{
    public Mirror() : base()
    {
        weaponName = "Mirror";
        description = "Light: Short Invincibility + Pushback\nHeavy: Summon 4 small mirror\nSpecial: Mirror Bomb";
        animPath = basePath + weaponName + ".";

        specialCooldown = Random.Range(5, 10);
        baseStrength = 3;
    }

    public override void LightAttack(Animator anim)
    {
        anim.Play(animPath + "MirrorLight");
    }

    public override void HeavyAttack(Animator anim)
    {
        anim.Play(animPath + "MirrorHeavy");
    }
    public override void SpecialAttack(Animator anim)
    {
        anim.Play(animPath + "MirrorSpecial");
    }
}
