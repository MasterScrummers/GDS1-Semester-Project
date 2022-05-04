using UnityEngine;

public class Jet : WeaponBase
{
    public Jet() : base()
    {
        weaponName = "Jet";

        specialCooldown = 1;//Random.Range(2, 4);
        int extra = Mathf.RoundToInt(specialCooldown * 0.5f);
        baseStrength = Random.Range(4 + extra, 7 + extra);

        description = "Light: Dash\nHeavy: Large Distance Dash\n Special: Dash with energy pulse";
        animPath = basePath + weaponName + ".";
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
