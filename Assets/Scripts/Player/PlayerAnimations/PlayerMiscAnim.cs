using UnityEngine;

public class PlayerMiscAnim : MonoBehaviour
{
    public PlayerAnim.AnimState animState { get; private set; } = PlayerAnim.AnimState.Idle;
    public PlayerAnim.JumpState jumpState { get; private set; } = PlayerAnim.JumpState.Waiting;

    [SerializeField] private GameObject[] projectiles; //Array of projectiles

    private InputController ic; // Input Controller
    private AudioController ac; // Audio Controller
    private PlayerInput pi; //The update the animation according to player input.
    private GameObject player;
    private Rigidbody2D rb;

    void Start()
    {
        ic = DoStatic.GetGameController<InputController>();
        ac = ic.GetComponent<AudioController>();
        pi = GetComponentInParent<PlayerInput>();
        rb = pi.GetComponent<Rigidbody2D>();
        player = DoStatic.GetPlayer();
    }

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

    private void RestGravityMultiplier()
    {
        pi.gravityMultiplier = pi.originalGravityMultiplier;

    }
    private void ChangeGravityMultiplier(float gravityMultiplier)
    {
        pi.gravityMultiplier = gravityMultiplier;
    }

    private void ChangeSpeed(float speed)
    {
        pi.speed = speed;
    }

    private void ResetSpeed()
    {
        pi.speed = pi.orignalspeed;
    }

    private void ActivateProjectile(int num)
    {
        Instantiate(projectiles[num], pi.firePoint.position, Quaternion.identity);
    }

    public void ResetRotation()
    {
        player.transform.eulerAngles = (Vector2)player.transform.eulerAngles;
    }
    
    private void ResetGravityScale()
    {
        rb.gravityScale = pi.originalGravity;
    }

}
