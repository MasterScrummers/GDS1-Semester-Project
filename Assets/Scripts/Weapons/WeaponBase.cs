using UnityEngine;

public abstract class WeaponBase
{
    public string weaponName { get; protected set; } = "Unknown Weapon Name."; //Name of the weapon.
    public string description { get; protected set; } = "No Description."; //How does the weapon function on each button?

    protected const string basePath = "Base Layer."; //The base path.
    protected string animPath; //The animation path, to keep things simple.

    public int baseStrength { get; protected set; } = 1; //The strength of the weapon.
    public int specialCooldown { get; protected set; } = 10; //The cooldown of the weapon.

    public WeaponBase() {}

    /// <summary>
    /// Meant to be overridden for the lignt attack.
    /// </summary>
    public virtual void LightAttack(Animator anim) {}

    /// <summary>
    /// Meant to be overridden for the heavy attack.
    /// </summary>
    public virtual void HeavyAttack(Animator anim) { }

    /// <summary>
    /// Meant to be overridden for the special attack.
    /// </summary>
    public virtual void SpecialAttack(Animator anim) {}

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
}
