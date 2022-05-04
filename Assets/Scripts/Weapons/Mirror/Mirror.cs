using UnityEngine;

public class Mirror : WeaponBase
{
    public Mirror() : base()
    {
        weaponName = "Mirror";

        description = "Light: Shield\nHeavy: Invincible\n Special: Spawn 4 Shield around ";
        animPath = basePath + weaponName + ".";
    }

    public override void SpecialAttack(Animator anim)
    {
        anim.Play(animPath + "MirrorSpecial");
    }
}
