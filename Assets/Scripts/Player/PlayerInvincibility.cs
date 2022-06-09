using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerInvincibility : MonoBehaviour
{
    private SpriteRenderer sprite;
    public bool invincible { get; private set; } = false;
    private bool allowFlashing = false;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = true;
    }

    void Update()
    {
        sprite.enabled = !invincible || !allowFlashing || !sprite.enabled;
    }

    public void SetPlayerInvincible(bool invincible, bool allowFlashing = true)
    {
        this.invincible = invincible;
        Physics2D.IgnoreLayerCollision(6, 7, invincible);
        this.allowFlashing = allowFlashing;
    }
}
