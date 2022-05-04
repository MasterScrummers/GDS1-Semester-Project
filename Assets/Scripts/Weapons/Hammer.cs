using UnityEngine;

public class Hammer : WeaponBase
{
    // Start is called before the first frame update
    public Hammer() : base()
    {
        weaponName = "Hammer";

        specialCooldown = Random.Range(4, 12);
        int extra = Mathf.RoundToInt(specialCooldown * 0.5f);
        baseStrength = Random.Range(3 + extra, 12 + extra);
        description = "Light: Smash\nHeavy: Spin(Hold)\nSpecial: Enhanced Attack";
        animPath = basePath + weaponName + "."; //Don't forget the dot!
    }

    public override void LightAttack(Animator anim)
    {
        anim.Play(animPath + "HammerLight");
    }

    public override void HeavyAttack(Animator anim)
    {
        anim.Play(animPath + "HammerHeavy");
    }

    public override void SpecialAttack(Animator anim)
    {
        anim.Play(animPath + "HammerSpecial");
    }
}
