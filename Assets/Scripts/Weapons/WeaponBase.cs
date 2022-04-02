using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected string weaponName = "Unknown Weapon Name."; //Name of the weapon.
    protected string description = "No Description."; //How does the weapon function on each button?
    public enum Affinity { water, fire, grass }; //All the weapon types are here. Any changes must have been discussed.
    protected Affinity weaponType = Affinity.water; //The weapon's typing.

    private int affinityNum; //The number of affinities.
    private Affinity weaponWeakness; //Weapon's weakness.

    /// <summary>
    /// Call base.start() last in children.
    /// </summary>
    protected virtual void Start()
    {
        affinityNum = typeof(Affinity).GetEnumValues().Length;
        weaponWeakness = (Affinity)(((int)weaponType + 1) % affinityNum);
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
