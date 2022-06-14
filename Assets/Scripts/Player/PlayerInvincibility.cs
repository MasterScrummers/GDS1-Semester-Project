#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;

public class PlayerInvincibility : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    public bool invincible { get; private set; } = false;
    public bool allowFlashing { get; private set; } = false;

    void Start()
    {
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
