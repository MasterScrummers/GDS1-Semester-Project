using UnityEngine;

public class Sword : PlayerWeaponBase
{
    public Sword(OriginalValue<float> speed) : base("Sword", speed)
    {
        description = "Light: Slash\nHeavy: Stab\n Special: Spin";

        specialCooldown = 6;
        //int extra = Mathf.RoundToInt(specialCooldown * 0.5f);
        damage = Random.Range(9 , 12);
    }

    public override void LightAttack(Animator anim)
    {
        UpdateValues(1, new(15, 0), 0.17f, 0.2f, "SwordLight");
        base.LightAttack(anim);
    }

    public override void HeavyAttack(Animator anim)
    {
        UpdateValues(3, 2, new(20, 0), 1f, 0.5f, "SwordHeavy1");
        base.HeavyAttack(anim);
    }

    public override void SpecialAttack(Animator anim)
    {
        UpdateValues(15, 2, new(15, 0), 0.5f, 0.3f, "SwordSpecial1");
        base.SpecialAttack(anim);
    }
}
