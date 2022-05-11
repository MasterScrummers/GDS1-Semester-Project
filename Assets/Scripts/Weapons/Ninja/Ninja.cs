using UnityEngine;

public class Ninja : WeaponBase
{
    public Ninja() : base()
    {
        weaponName = "Ninja";
        description = "Light: Kunai\nHeavy: Invincible\n Special: Spawn 4 Shield around ";
        animPath = basePath + weaponName + ".";
        
        specialCooldown = Random.Range(5, 10);
        int extra = Mathf.RoundToInt(specialCooldown * 0.1f);
        baseStrength = Random.Range(1 + extra, 2 + extra);
    }

    public override void LightAttack(Animator anim)
    {
        anim.Play(animPath + "NinjaLight");
    }
}
