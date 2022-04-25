using System.Collections.Generic;
using UnityEngine;

public class AttackDealer : MonoBehaviour
{
    public int strength = 1; //The strength the hit, should be controlled by another script.
    private Dictionary<HealthComponent, int> hitHistory; //A history of all the hit enemies.
    private Animator anim;

    private void Start()
    {
        hitHistory = new Dictionary<HealthComponent, int>();
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Should be called as an animation event.
    /// Simply cleans the dictionary to allow the same enemy to be hit again.
    /// </summary>
    private void AttackStart()
    {
        hitHistory.Clear();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        HealthComponent hp = collision.GetComponent<HealthComponent>();
        if (!hp)
        {
            return;
        }

        hp.TakeDamage(strength);
        if (anim)
        {
            hitHistory.Add(hp, 0);
        }
    }
}
