using UnityEngine;

public abstract class WeaponBase
{
    public string weaponName { get; protected set; } = "Unknown Weapon Name."; //Name of the weapon.
    public string description { get; protected set; } = "No Description."; //How does the weapon function on each button?
    
    protected const string basePath = "Base Layer."; //The base path.
    protected string animPath; //The animation path, to keep things simple.

    public enum Affinity { water, fire, grass }; //All the weapon types are here. Any changes must have been discussed.

    public Affinity weaponType { get; private set; } = Affinity.water; //The weapon's typing.
    public Color32 weaponColour { get; private set; } = Color.white; //Colour of the weapon.
    public int baseStrength { get; protected set; } = 1; //The strength of the weapon.
    public int specialCooldown { get; protected set; } = 10; //The cooldown of the weapon.
    public float knockbackStr { get; protected set; } = 10; //The knockback strength of the weapon.

    public WeaponBase()
    {
        int affinityNum = typeof(Affinity).GetEnumValues().Length;
        weaponType = (Affinity)Random.Range(0, affinityNum);
        weaponColour = DoStatic.GetGameController<VariableController>().GetColor(weaponType);
    }

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
}
