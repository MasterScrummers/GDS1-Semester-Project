using UnityEngine;

public class Sword : WeaponBase
{
    protected override void Start()
    {
        weaponName = "Sword";
        description = "Basic Weapon";
        weaponType = Affinity.water;
        base.Start();
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
