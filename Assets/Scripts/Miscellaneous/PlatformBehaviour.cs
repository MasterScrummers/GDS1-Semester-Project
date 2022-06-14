using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    private CompositeCollider2D platform;
    private InputController ic;
    private Collider2D playerCollider;

    void Start()
    {
        platform = GetComponent<CompositeCollider2D>();
        ic = DoStatic.GetGameController<InputController>();
        playerCollider = DoStatic.GetPlayer<PlayerBody>().col;
    }

    void Update()
    {
        if (platform && playerCollider)
        {
            Physics2D.IgnoreCollision(playerCollider, platform, ic.GetAxisRawValues("Movement", "Vertical") == -1);
        }
    }
}
