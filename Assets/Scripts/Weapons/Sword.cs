using UnityEngine;

public class Sword : WeaponBase
{
    public Sword() : base()
    {
        weaponName = "Sword";
        description = "Basic Weapon";
        animPath = basePath + "Sword."; //Don't forget the dot!
    }

    public override void LightAttack(Animator anim)
    {
        anim.Play(animPath + "SwordLight");
    }

    public override void HeavyAttack(Animator anim)
    {
        anim.Play(animPath + "SwordHeavy");
    }

    public override void SpecialAttack(Animator anim)
    {
        anim.Play(animPath + "SwordSpecial");
    }
}
