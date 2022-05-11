using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerInvincibility : MonoBehaviour
{
    private SpriteRenderer sprite;
    private float timer;
    public bool invincible { get; private set; } = false;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = true;
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

    public void StartInvincible(float invincibleLength)
    {
        timer = invincibleLength;
        Physics2D.IgnoreLayerCollision(6, 7, true);
        invincible = true;
    }
}
