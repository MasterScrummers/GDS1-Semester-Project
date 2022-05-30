using UnityEngine;

public class Sword : WeaponBase
{
    public Sword() : base()
    {
        weaponName = "Sword";
        description = "Light: Slash\nHeavy: Stab\n Special: Spin";
        animPath = basePath + weaponName + "."; //Don't forget the dot!

        specialCooldown = Random.Range(4, 9);
        int extra = Mathf.RoundToInt(specialCooldown * 0.5f);
        baseStrength = Random.Range(3 + extra, 8 + extra);
        knockbackStr = new(10f, 0);
        invincibilityTime = 0.3f;
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
