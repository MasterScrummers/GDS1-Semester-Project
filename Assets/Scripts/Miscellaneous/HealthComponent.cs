using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public int startMaxHealth = 2; //For editor to set the max health.
    public int maxHealth { get; private set; } //The max health.
    public int health { get; private set; } //The current health.

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = startMaxHealth;
        health = maxHealth;
    }

    /// <summary>
    /// Resets the hp.
    /// </summary>
    public void Restart()
    {
        maxHealth = startMaxHealth;
        health = maxHealth;
    }

    /// <summary>
    /// Change the health value.
    /// </summary>
    /// <param name="amount">Change value by given amount.</param>
    public void TakeDamage(int amount)
    {
        health = Mathf.Clamp(health - amount, 0, maxHealth);
    }

    public void HealDamage(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }

    public float GetPercentage()
    {
        return (float)health / maxHealth;
    }
}
