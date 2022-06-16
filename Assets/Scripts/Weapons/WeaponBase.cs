#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;

public abstract class WeaponBase
{
    public string weaponName { get; private set; } //Name of the weapon.
    public string description { get; protected set; } = "No Description."; //How does the weapon function on each button?
    protected string animPath; //The animation path, to keep things simple.

    public int strength { get; protected set; } = 1; //The strength of the weapon.
    public int specialCooldown { get; protected set; } = 3; //The cooldown of the weapon.
    public int strengthMult { get; protected set; } = 1; //The strength multiplier on the weapon.
    public Vector2 knockback { get; protected set; } = Vector2.one; //The knockback to give.
    public float hitInterval { get; protected set; } = 0.3f;
    public float stunTime { get; protected set; } = 0.5f;
    public string sfx { get; protected set; } = "";

    public WeaponBase(string name) {
        weaponName = name;
        animPath = "Base Layer." + weaponName + ".";
    }

    /// <summary>
    /// Be sure to call this last in the override
    /// </summary>
    /// <param name="anim">The animator to call the animation.</param>
    public virtual void LightAttack(Animator anim) {
        anim.Play(animPath + weaponName + "Light");
    }

    /// <summary>
    /// Be sure to call this last in the override
    /// </summary>
    /// <param name="anim">The animator to call the animation.</param>
    public virtual void HeavyAttack(Animator anim) {
        anim.Play(animPath + weaponName + "Heavy");
    }

    /// <summary>
    /// Be sure to call this last in the override
    /// </summary>
    /// <param name="anim">The animator to call the animation.</param>
    public virtual void SpecialAttack(Animator anim) {
        anim.Play(animPath + weaponName + "Special");
    }

    public static WeaponBase RandomWeapon()
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
