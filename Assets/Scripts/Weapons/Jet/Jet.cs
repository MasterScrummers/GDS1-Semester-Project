using UnityEngine;

public class Jet : WeaponBase
{
    public Jet() : base()
    {
        weaponName = "Jet";
        description = "Light: Dash\nHeavy: Long Dash\n Special: Dash Explosion";
        animPath = basePath + weaponName + "."; 

        specialCooldown = Random.Range(3, 5);
        baseStrength = Random.Range(5 , 8);
        knockbackStr = new(30f, 0);
        invincibilityTime = 0.2f;
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
