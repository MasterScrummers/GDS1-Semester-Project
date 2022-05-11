using UnityEngine;

public class Ninja : WeaponBase
{
    public Ninja() : base()
    {
        weaponName = "Ninja";
        description = "Light: Kunai\nHeavy: Shotgun like Kunai\n Special: Kunai bullet hell";
        animPath = basePath + weaponName + ".";
        
        specialCooldown = Random.Range(5, 10);
        int extra = Mathf.RoundToInt(specialCooldown * 0.1f);
        baseStrength = 3;
        invincibilityTime = 0.5f;
    }

    public override void LightAttack(Animator anim)
    {
        anim.Play(animPath + "NinjaLight");
    }

    public override void HeavyAttack(Animator anim)
    {
        anim.Play(animPath + "NinjaHeavy");
    }

    public override void SpecialAttack(Animator anim)
    {
        anim.Play(animPath + "NinjaSpecial");
    }
}
