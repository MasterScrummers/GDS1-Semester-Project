#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;

public class PlayerWeaponBase : WeaponBase
{
    public string weaponName { get; private set; } //Name of the weapon.
    public string description { get; protected set; } = "No Description."; //How does the weapon function on each button?
    protected string animPath; //The animation path, to keep things simple.

    public int specialCooldown { get; protected set; } = 3; //The cooldown of the weapon.

    public PlayerWeaponBase(string weaponName)
    {
        this.weaponName = weaponName;
        animPath = "Base Layer." + weaponName + ".";
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

    public static PlayerWeaponBase RandomWeapon()
    {
        return Random.Range(1, 7) switch
        {
            1 => new Sword(),
            2 => new Hammer(),
            3 => new Cutter(),
            4 => new Mirror(),
            5 => new Jet(),
            6 => new Ninja(),
            _ => new Sword(),
        };
    }

    protected void UpdateValues(int strengthMult, Vector2 knockback, float hitInterval, float stunTime, string sfx = "")
    {
        this.strengthMult = strengthMult;
        this.knockback = knockback;
        this.hitInterval = hitInterval;
        this.stunTime = stunTime;
        this.sfx = sfx;
    }
}
