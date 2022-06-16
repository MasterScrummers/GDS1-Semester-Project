#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;

public abstract class WeaponBase
{
    public int strength { get; protected set; } = 1; //The strength of the weapon.
    public int strengthMult { get; protected set; } = 1; //The strength multiplier on the weapon.
    public Vector2 knockback { get; protected set; } = Vector2.one; //The knockback to give.
    public float hitInterval { get; protected set; } = 0.3f;
    public float stunTime { get; protected set; } = 0.5f;
    public string sfx { get; protected set; } = "";
}
