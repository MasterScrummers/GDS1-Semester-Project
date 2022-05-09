using UnityEngine;

public class Ninja : WeaponBase
{
    public Ninja() : base()
    {
        weaponName = "Ninja";
        description = "Light: Kunai\nHeavy: Invincible\n Special: Spawn 4 Shield around ";
        animPath = basePath + weaponName + ".";
    }

    public override void LightAttack(Animator anim)
    {
        anim.Play(animPath + "NinjaLight");
    }
}
