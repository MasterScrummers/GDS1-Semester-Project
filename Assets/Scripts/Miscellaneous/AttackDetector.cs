using System.Collections.Generic;
using UnityEngine;

public class AttackDetector : MonoBehaviour
{
    public int strength = 1; //The strength the hit, should be controlled by another script.
    private Dictionary<HealthComponent, int> hitHistory; //A history of all the hit enemies.
    
    private PlayerAnim playerAnim; //Kirby's animation for the attack

    private void Start()
    {
        hitHistory = new Dictionary<HealthComponent, int>();

        playerAnim = DoStatic.GetPlayer().GetComponentInChildren<PlayerAnim>();
    }

    /// <summary>
    /// Should be called as an animation event.
    /// Simply cleans the dictionary to allow the same enemy to be hit again.
    /// </summary>
    public void AttackStart()
    {
        hitHistory.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthComponent hp = collision.GetComponent<HealthComponent>();
        if (hp && !hitHistory.ContainsKey(hp))
        {
            if (collision.CompareTag("Player"))
            {
                if (!playerAnim.invincible)
                {
                    hp.TakeDamage(strength);
                    playerAnim.TakeDamage(collision.transform.position.x < transform.position.x ? -1 : 1);
                }
            } else {
                hp.TakeDamage(strength);
                hitHistory.Add(hp, 0);
            }
        }

        
    }
}
