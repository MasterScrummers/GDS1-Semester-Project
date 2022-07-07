#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;

public class PlayerInvincibility : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [field: SerializeField] public bool invincible { get; private set; } = false;
    [field: SerializeField] public bool allowFlashing { get; private set; } = false;

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
        this.allowFlashing = allowFlashing;
    }
}
