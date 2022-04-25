using UnityEngine;

public class Hammer : WeaponBase
{
    // Start is called before the first frame update
    public Hammer() : base()
    {
        weaponName = "Hammer";
        description = "Light: Smash \nHeavy: Spin(Hold)\nSpecial: Enhanced Attack";
        animPath = basePath + "Hammer.";
       
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
