using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [field: SerializeField] public int maxHealth { get; private set; } = 4; //The max health.
    public int health { get; private set; } //The current health.

    void Start()
    {
        SetHP();
    }

    /// <summary>
    /// Offsets the current health by given amount.
    /// </summary>
    /// <param name="amount">Can be negative or positive.</param>
    public void OffsetHP(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }

    /// <summary>
    /// Sets the curent health to amount given.
    /// If amount given is none, fully restore the health.
    /// </summary>
    /// <param name="healthAmount">The amount to set the health to.</param>
    public void SetHP(int healthAmount = int.MaxValue)
    {
        health = Mathf.Clamp(healthAmount, 0, maxHealth);
    }

    /// <summary>
    /// Sets the max health and current health to amount given.
    /// </summary>
    /// <param name="max">The max amount of health.</param>
    /// <param name="healthAmount">The amount of health to set to.</param>
    public void SetHP(int max, int healthAmount)
    {
        maxHealth = max;
        SetHP(healthAmount);
    }

    /// <summary>
    /// Sets the mex health.
    /// Health will change accordingly.
    /// </summary>
    /// <param name="max">The max health to set to.</param>
    public void SetMaxHP(int max)
    {
        SetHP(max, maxHealth);
    }

    /// <summary>
    /// Get the current health as a percentage.
    /// </summary>
    /// <returns>A percentage in decimal format.</returns>
    public float GetPercentage()
    {
        return (float)health / maxHealth;
    }
}
