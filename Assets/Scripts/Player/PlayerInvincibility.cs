using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerInvincibility : MonoBehaviour
{
    private SpriteRenderer sprite;
    private HealthComponent health; //To set the invincibility of the health.

    [SerializeField] private float invincibilityTimer = 1.5f;
    private float timer;

    private bool invincible = false;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = true;
        health = DoStatic.GetPlayer<HealthComponent>();
    }

    void Update()
    {
        if (!invincible)
        {
            return;
        }

        timer -= Time.deltaTime;
        invincible = timer > 0;
        sprite.enabled = invincible ? !sprite.enabled : true;
        Physics2D.IgnoreLayerCollision(6, 7, invincible);
    }

    public void StartInvincible()
    {
        timer = invincibilityTimer;
        Physics2D.IgnoreLayerCollision(6, 7, true);
        invincible = true;
    }
}
