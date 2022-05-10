using UnityEngine;

public abstract class WeaponBase
{
    public string weaponName { get; protected set; } = "Unknown Weapon Name."; //Name of the weapon.
    public string description { get; protected set; } = "No Description."; //How does the weapon function on each button?
    
    protected const string basePath = "Base Layer."; //The base path.
    protected string animPath; //The animation path, to keep things simple.
    protected int specialCooldown = 10; //The cooldown of the weapon.
    protected float knockbackStrength = 1f; //The knockback of the weapon.

    public enum Affinity { water, fire, grass }; //All the weapon types are here. Any changes must have been discussed.
    protected Affinity weaponType = Affinity.water; //The weapon's typing.
    public Color32 weaponColour { get; private set; } = Color.white; //Colour of the weapon.

    private Affinity weaponWeakness; //Weapon's weakness.
    public int baseStrength { get; protected set; } = 1;

    public WeaponBase()
    {
        int affinityNum = typeof(Affinity).GetEnumValues().Length;
        weaponType = (Affinity)Random.Range(0, affinityNum);
        weaponWeakness = (Affinity)(((int)weaponType + 1) % affinityNum);
        weaponColour = GenerateWeaponColour();
    }

    /// <summary>
    /// Compares two affinities.
    /// </summary>
    /// <param name="enemyType">The affinity to compare this weapon to.</param>
    /// <returns>A multiplier. Should be used for damage.</returns>
    public float AffinityCompare(Affinity enemyType)
    {
        return enemyType == weaponWeakness ? 0.75f : enemyType == weaponType ? 1 : 1.25f;
    }

    private Color GenerateWeaponColour()
    {
        switch(weaponType)
        {
            case Affinity.fire:
                return new Color32(183, 18, 52, 255);

            case Affinity.grass:
                return new Color32(0, 155, 72, 255);

            case Affinity.water:
                return new Color32(0, 70, 173, 255);

            default:
                return Color.white;
        }
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

    /// <summary>
    /// Gets the weapon cooldown.
    /// </summary>
    /// <returns>The weapon cooldown.</returns>
    public float GetWeaponCooldown()
    {
        return specialCooldown;
    }

}
