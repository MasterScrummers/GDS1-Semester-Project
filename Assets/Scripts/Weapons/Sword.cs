using UnityEngine;

public class Sword : WeaponBase
{
    public Sword() : base()
    {
        weaponName = "Sword";
        description = "Basic Weapon";
    }

    public override void LightAttack(Animator anim)
    {
        anim.Play("SwordLight");
    }

    public override void HeavyAttack(Animator anim)
    {
        anim.Play("SwordHeavy");
    }

    public override void SpecialAttack(Animator anim)
    {
        anim.Play("SwordSpecial");
    }
}
