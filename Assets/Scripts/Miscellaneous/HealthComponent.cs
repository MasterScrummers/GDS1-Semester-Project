using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public int startMaxHealth = 2; //For editor to set the max health.
    private int mhp; //The max health.
    public int hp { get; private set; } //The current health.

    // Start is called before the first frame update
    void Start()
    {
        mhp = startMaxHealth;
        hp = mhp;
    }

    /// <summary>
    /// Resets the hp.
    /// </summary>
    public void Restart()
    {
        hp = mhp;
    }

    /// <summary>
    /// Change the health value.
    /// </summary>
    /// <param name="amount">Change value by given amount.</param>
    public void TakeDamage(int amount)
    {
        hp = Mathf.Clamp(hp - amount, 0, mhp);
    }
}
