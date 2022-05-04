using UnityEngine;

public class Cutter : WeaponBase
{
    public Cutter() : base()
    {
        weaponName = "Cutter";
        specialCooldown = Random.Range(3, 8);
        int extra = Mathf.RoundToInt(specialCooldown * 0.5f);
        baseStrength = Random.Range(4 + extra, 5 + extra);
        description = "Light: Slash\nHeavy: Jump Attack\nSpecial: Throw Cutter";
        animPath = basePath + weaponName + "."; //Don't forget the dot!
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
