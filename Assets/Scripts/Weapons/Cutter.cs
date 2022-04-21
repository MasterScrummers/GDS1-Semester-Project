using UnityEngine;

public class Cutter : WeaponBase
{
    public Cutter() : base()
    {
        weaponName = "Cutter";
        description = "Light: \nHeavy: \nSpecial: ";
        animPath = basePath + "Cutter.";
    }

    public override void LightAttack(Animator anim)
    {
        anim.Play(animPath + "CutterLight");
    }

    public override void HeavyAttack(Animator anim)
    {
        anim.Play(animPath + "CutterHeavy");
        
    }

    public override void SpecialAttack(Animator anim)
    {
        anim.Play(animPath + "CutterSpecial");
    }

}
