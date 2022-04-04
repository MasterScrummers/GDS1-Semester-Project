using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int health = 1;
    [SerializeField] protected WeaponBase.Affinity type;

    // Start is called before the first frame update
    protected virtual void Start() {}

    // Update is called once per frame
    protected virtual void Update()
    {
        if (health <= 0) {
            Death();
        }
    }

    /// <summary>
    /// Meant to be overridden for movement script. This is already called in Update()
    /// </summary>
    protected virtual void Move() {}

    /// <summary>
    /// Meant to be overridden for attack script
    /// </summary>
    protected virtual void Attack() {}

    /// <summary>
    /// Called when health <= 0. Can override for a unique Death script, otherwise just destroys the GameObject.
    /// </summary>
    protected virtual void Death() 
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Can be overridden for taking damage
    /// </summary>
    /// <param name="damage">
    /// The amount of damage the enemy should take
    /// </param>
    protected virtual void TakeDamage(int damage)
    {
        health -= damage;
    }
}
