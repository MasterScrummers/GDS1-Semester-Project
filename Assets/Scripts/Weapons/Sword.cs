using UnityEngine;

public class Sword : PlayerWeaponBase
{
    public Sword() : base("Sword")
    {
        description = "Light: Slash\nHeavy: Stab\n Special: Spin";

        specialCooldown = Random.Range(5, 9);
        int extra = Mathf.RoundToInt(specialCooldown * 0.5f);
        strength = Random.Range(5 + extra, 8 + extra);
    }

    public override void LightAttack(Animator anim)
    {
        UpdateValues(1, new(15, 0), 0.2f, 0.25f);
        base.LightAttack(anim);
    }

    public override void HeavyAttack(Animator anim)
    {
        //set player speed to 3 (slightly slower)
        UpdateValues(2, new(20, 0), 0.3f, 0.25f);
        base.HeavyAttack(anim);
    }

    public override void SpecialAttack(Animator anim)
    {
        //set player speed to 15 (Much faster)
        UpdateValues(1, new(-15, -15), 0.1f, 0.5f);
        anim.Play(animPath + weaponName + "Special");
    }
}
