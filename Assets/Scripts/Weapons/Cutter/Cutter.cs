using UnityEngine;

public class Cutter : WeaponBase
{
    public Cutter() : base("Cutter")
    {
        description = "Light: Slash\nHeavy: Range Cutter\nSpecial: Cutter Hell";

        specialCooldown = Random.Range(3, 5);
        int extra = Mathf.RoundToInt(specialCooldown * 0.5f);
        strength = Random.Range(5 + extra, 7 + extra);

        knockback = new(10, 0);
        hitInterval = 0.3f;
        stunTime = 0.3f;
    }

    public override void LightAttack(Animator anim)
    {
        //set player speed to 7 (slightly faster)
        UpdateValues(1, knockback, hitInterval, stunTime);
        base.LightAttack(anim);
    }

    public override void HeavyAttack(Animator anim)
    {
        UpdateValues(2, knockback, hitInterval, stunTime);
        //set player speed to 2 (slower)
        base.HeavyAttack(anim);
    }

    public override void SpecialAttack(Animator anim)
    {
        UpdateValues(3, knockback, hitInterval, stunTime);
        base.SpecialAttack(anim);
    }
}
