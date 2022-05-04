using UnityEngine;

public class Sword : WeaponBase
{
    public Sword() : base()
    {
        weaponName = "Sword";
        specialCooldown = Random.Range(4, 9);
        int extra = Mathf.RoundToInt(specialCooldown * 0.5f);
        baseStrength = Random.Range(3 + extra, 8 + extra);
        description = "Light: Slash\nHeavy: Stab\n Special: Spin";
        animPath = basePath + weaponName + "."; //Don't forget the dot!
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
