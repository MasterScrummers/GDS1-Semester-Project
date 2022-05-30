using UnityEngine;

public class Jet : WeaponBase
{
    public Jet() : base()
    {
        weaponName = "Jet";
        description = "Light: Dash\nHeavy: Long Dash\n Special: Dash with energy pulse";
        animPath = basePath + weaponName + "."; 

        specialCooldown = Random.Range(2, 4);
        int extra = Mathf.RoundToInt(specialCooldown * 0.5f);
        baseStrength = Random.Range(4 + extra, 7 + extra);
        knockbackStr = new(18f, 0);
    }

    public override void LightAttack(Animator anim)
    {
        anim.Play(animPath + "JetLight");
    }

    public override void HeavyAttack(Animator anim)
    {
        anim.Play(animPath + "JetHeavy");
    }

    public override void SpecialAttack(Animator anim)
    {
        anim.Play(animPath + "JetSpecial");
    }
}
