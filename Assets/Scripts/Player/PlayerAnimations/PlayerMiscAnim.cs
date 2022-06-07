#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;

public class PlayerMiscAnim : MonoBehaviour
{
    public PlayerAnim.AnimState animState { get; private set; } = PlayerAnim.AnimState.Idle;
    public PlayerAnim.JumpState jumpState { get; private set; } = PlayerAnim.JumpState.Waiting;

    private InputController ic; // Input Controller
    private AudioController ac; // Audio Controller
    private PlayerInput pi; //The update the animation according to player input.
    private JumpComponent jump;
    private GameObject player;
    private Rigidbody2D rb;

    void Start()
    {
        ic = DoStatic.GetGameController<InputController>();
        ac = ic.GetComponent<AudioController>();
        
        pi = GetComponentInParent<PlayerInput>();
        jump = pi.GetComponent<JumpComponent>();

        rb = pi.GetComponent<Rigidbody2D>();
        player = DoStatic.GetPlayer();
    }

#pragma warning disable IDE0051
    private void SetAnimState(PlayerAnim.AnimState state)
    {
        animState = state;
    }

    private void SetJumpState(PlayerAnim.JumpState state)
    {
        jumpState = state;
    }

    private void SetReasonLock(string ID)
    {
        ic.SetID(ID, false);

    }

    public void SetReasonUnlock(string ID)
    {
        ic.SetID(ID, true);
    }

    private void PlaySound(string clipName)
    {
        ac.PlaySound(clipName);
    }

    private void ChangeGravityFall(float gravityMultiplier)
    {
        jump.fallGravity.value = gravityMultiplier;
    }

    private void ResetGravityFall()
    {
        jump.fallGravity.Reset();
    }

    private void ChangeSpeed(float speed)
    {
        pi.speed.value = speed;
    }

    private void ResetSpeed()
    {
        pi.speed.Reset();
    }

    public void ResetRotation()
    {
        player.transform.eulerAngles = (Vector2)player.transform.eulerAngles;
    }
}
