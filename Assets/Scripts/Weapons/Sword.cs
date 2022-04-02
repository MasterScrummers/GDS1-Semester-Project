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
        anim.Play("KirbyLightSwordAttack");
    }

    public override void HeavyAttack(Animator anim)
    {
    }

    public override void SpecialAttack(Animator anim)
    {
    }
}
