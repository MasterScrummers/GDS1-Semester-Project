using UnityEngine;

public class Cutter : WeaponBase
{
    public Cutter() : base()
    {
        weaponName = "Cutter";
        description = "Light: Slash\nHeavy: Jump Attack\nSpecial: Throw Cutter";
        animPath = basePath + weaponName + "."; 

        specialCooldown = Random.Range(3, 5);
        int extra = Mathf.RoundToInt(specialCooldown * 0.5f);
        baseStrength = Random.Range(4 + extra, 5 + extra);
        knockbackStr = 8f;
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
