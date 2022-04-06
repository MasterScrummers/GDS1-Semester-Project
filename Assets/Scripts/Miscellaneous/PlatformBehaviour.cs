using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    private BoxCollider2D platform;
    private InputController ic;
    private Collider2D playerCollider;

    void Start()
    {
        platform = GetComponent<BoxCollider2D>();
        ic = DoStatic.GetGameController<InputController>();
        playerCollider = DoStatic.GetPlayer().GetComponent<Collider2D>();
    }

    void Update()
    {
        Physics2D.IgnoreCollision(playerCollider, platform, ic.axisRawValues["Vertical"] == -1);
    }
}
