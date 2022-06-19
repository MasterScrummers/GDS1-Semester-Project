#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;

public abstract class WeaponBase
{
    public int damage { get; protected set; } = 1; //The strength of the weapon.
    public Vector2 knockback { get; protected set; } = Vector2.one; //The knockback to give.
    public float hitInterval { get; protected set; } = 0.3f; //If the collision is still connected after time, hits again.
    public string sfx { get; protected set; } = "";
}
