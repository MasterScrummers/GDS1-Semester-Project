#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;

public abstract class PlayerWeaponBase : WeaponBase
{
    public string weaponName { get; private set; } //Name of the weapon.
    public string description { get; protected set; } = "No Description."; //How does the weapon function on each button?
    protected string animPath; //The animation path, to keep things simple.

    public int strength { get; protected set; } = 1; //The strength of the weapon.
    public int specialCooldown { get; protected set; } = 3; //The cooldown of the weapon.
    public int strengthMult { get; protected set; } = 1; //The strength multiplier on the weapon.
    public float stunTime { get; protected set; } = 0.5f; //For enemy AI to freeze temporarily.
    public OriginalValue<float> speed { get; protected set; } //Controls the player's speed.

    public PlayerWeaponBase(string weaponName, OriginalValue<float> speed)
    {
        this.weaponName = weaponName;
        animPath = "Base Layer." + weaponName + ".";
        this.speed = speed;
    }

    /// <summary>
    /// Be sure to call this last in the override
    /// </summary>
    /// <param name="anim">The animator to call the animation.</param>
    public virtual void LightAttack(Animator anim)
    {
        anim.Play(animPath + weaponName + "Light");
    }

    /// <summary>
    /// Be sure to call this last in the override
    /// </summary>
    /// <param name="anim">The animator to call the animation.</param>
    public virtual void HeavyAttack(Animator anim)
    {
        anim.Play(animPath + weaponName + "Heavy");
    }

    /// <summary>
    /// Be sure to call this last in the override
    /// </summary>
    /// <param name="anim">The animator to call the animation.</param>
    public virtual void SpecialAttack(Animator anim)
    {
        anim.Play(animPath + weaponName + "Special");
    }

    public static PlayerWeaponBase RandomWeapon(OriginalValue<float> speed)
    {
        return Random.Range(1, 7) switch
        {
            1 => new Sword(speed),
            2 => new Hammer(speed),
            3 => new Cutter(speed),
            4 => new Mirror(speed),
            5 => new Jet(speed),
            6 => new Ninja(speed),
            _ => new Sword(speed),
        };
    }

    protected void UpdateValues(int strengthMult, Vector2 knockback, float hitInterval, float stunTime, string sfx = "")
    {
        UpdateValues(speed.originalValue, strengthMult, knockback, hitInterval, stunTime, sfx);
    }

    protected void UpdateValues(float speed, int strengthMult, Vector2 knockback, float hitInterval, float stunTime, string sfx = "")
    {
        this.speed.value = speed;
        this.strengthMult = strengthMult;
        this.knockback = knockback;
        this.hitInterval = hitInterval;
        this.stunTime = stunTime;
        this.sfx = sfx;
    }
}
