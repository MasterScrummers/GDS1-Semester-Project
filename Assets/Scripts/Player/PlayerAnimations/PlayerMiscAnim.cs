#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable IDE0051
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
    private PoolController poolController;
    private Collider2D col; //The collider of the player. Is disabled upon death.
    private AttackDealer attacker;
    private PlayerInvincibility invincibility;

    void Start()
    {
        ic = DoStatic.GetGameController<InputController>();
        poolController = ic.GetComponent<PoolController>();
        ac = ic.GetComponent<AudioController>();
        
        pi = GetComponentInParent<PlayerInput>();
        jump = pi.GetComponent<JumpComponent>();
        col = pi.GetComponent<Collider2D>();
        rb = pi.GetComponent<Rigidbody2D>();

        player = DoStatic.GetPlayer();
        attacker = GetComponent<AttackDealer>();
        invincibility = GetComponent<PlayerInvincibility>();

        ms = shield.GetComponent<MirrorShieldMovement>();
    }

    private void Update()
    {
        void CutterUpdate()
        {
            if (nextWave != 0 && DoStatic.GetChildren(cutterPivot.transform).Length == 0)
            {
                CutterSpecial(nextWave);
                nextWave = nextWave >= 32 ? 0 : nextWave;
            }
        }

        void NinjaUpdate()
        {
            ninjaTimer.Update(Time.deltaTime);
            if (bulletHellWaves-- > 0 && ninjaTimer.tick == 0)
            {
                ninjaTimer.Reset();
                float num = 10;
                for (int i = 0; i < num; i++)
                {
                    SpawnKunai((360 / num * i) + angle).GetComponent<AttackDealer>().UpdateAttackDealer(pi.specialWeapon);
                }
                angle += 20;
            }
        }

        CutterUpdate();
        NinjaUpdate();
    }

    private void AnimMethod(string methodName)
    {
        switch (methodName)
        {
            case "IdleStart":
                SetAnimState(PlayerAnim.AnimState.Idle);
                pi.isSliding = false;
                break;

            case "WalkStart":
                SetAnimState(PlayerAnim.AnimState.Run);
                //ResetSpeed();
                break;

            case "JumpStart":
                SetAnimState(PlayerAnim.AnimState.Jump);
                SetJumpState(PlayerAnim.JumpState.StartJump);
                break;

            case "JumpFallStart":
                SetAnimState(PlayerAnim.AnimState.Jump);
                SetJumpState(PlayerAnim.JumpState.Descending);
                break;

            #region Sword
            case "SwordLightStart":
                SetAnimState(PlayerAnim.AnimState.LightAttack);
                attacker.SetKnockbackX(10);
                attacker.SetInvincibilityLength(0.3f);
                break;

            case "SwordHeavyStart":
                SetAnimState(PlayerAnim.AnimState.HeavyAttack);
                ChangeSpeed(3);
                attacker.SetStrengthMult(2);
                attacker.SetKnockbackX(15);
                attacker.SetInvincibilityLength(0.5f);
                break;

            case "SwordHeavyEnd":
                attacker.SetStrengthMult(1);
                SetReasonUnlock("Movement"); //Locked halfway through.
                ResetSpeed();
                break;

            case "SwordSpecialStart":
                SetAnimState(PlayerAnim.AnimState.SpecialAttack);
                ChangeSpeed(13);
                attacker.SetStrengthMult(2);
                //invincibility.StartAnimInvincible(1.2f);
                attacker.SetKnockbackX(8);
                attacker.SetInvincibilityLength(0.3f);
                break;

            case "SwordSpecialEnd":
                ResetSpeed();
                attacker.SetStrengthMult(1);
                break;
            #endregion

            #region Hammer
            case "HammerLightStart":
                SetAnimState(PlayerAnim.AnimState.LightAttack);
                //Lock movement replaced with slowed movement
                ChangeSpeed(3);
                attacker.SetKnockbackX(17);
                attacker.SetInvincibilityLength(0.3f);
                break;

            case "HammerHeavyStart":
                SetAnimState(PlayerAnim.AnimState.HeavyAttack);
                attacker.SetStrengthMult(2);
                attacker.SetKnockbackX(17);
                attacker.SetInvincibilityLength(0.3f);

                //Considering making Hammer Heavy have sliding of Jet Light.
                break;

            case "HammerHeavyEnd":
                attacker.SetStrengthMult(1);
                SetReasonUnlock("Movement"); //Gets locked halfway through
                break;

            case "HammerSpecialStart":
                SetAnimState(PlayerAnim.AnimState.SpecialAttack);
                attacker.SetInvincibilityLength(0.3f);

                ChangeSpeed(1);
                attacker.SetKnockbackX(10);
                PlaySound("HammerSpecial_0");
                break;

            case "HammerFlip":
                attacker.SetKnockbackX(27);
                attacker.SetKnockbackX(35);
                SetReasonLock("Movement");
                attacker.SetStrengthMult(3);
                break;

            case "HammerSpecialEnd":
                ResetSpeed();
                attacker.SetStrengthMult(1);
                SetReasonUnlock("Movement");
                break;
            #endregion

            #region Cutter
            case "CutterLightStart":
                CutterStart(PlayerAnim.AnimState.LightAttack, 7);
                break;

            case "CutterHeavyStart":
                CutterStart(PlayerAnim.AnimState.HeavyAttack, 2);
                break;
            #endregion

            #region Jet
            case "JetLightStart":
                JetStart(PlayerAnim.AnimState.LightAttack, DashDirection.Light);
                //Set invincible for 0.5 seconds
                break;

            case "JetHeavyStart":
                JetStart(PlayerAnim.AnimState.HeavyAttack, DashDirection.Heavy);
                attacker.SetStrengthMult(2);
                attacker.SetKnockbackX(40);
                //Set invincible for 1 second
                break;

            case "JetSpecialStart":
                JetStart(PlayerAnim.AnimState.SpecialAttack, DashDirection.Special);
                attacker.SetStrengthMult(3);
                attacker.SetKnockbackX(50);
                attacker.SetStunTime(1);
                attacker.SetInvincibilityLength(0.5f);
                //Set invincible for 1 second
                break;

            case "JetEnd":
                ResetGravityFall();
                SetReasonUnlock("Movement");
                attacker.SetStrengthMult(1);
                pi.isSliding = false;
                break;
            #endregion

            #region Mirror
            case "MirrorLightStart":
                SetAnimState(PlayerAnim.AnimState.LightAttack);
                //SetReasonLock("Movement");
                ChangeSpeed(3);
                break;

            case "MirrorLightActivate":
                //invincibility.StartAnimInvincible(0.5f);
                ChangeSpeed(2);
                shield.SetActive(true);
                ms.IsSpecialAttack = false;
                break;

            case "MirrorHeavyStart":
                SetAnimState(PlayerAnim.AnimState.HeavyAttack);
                SetReasonLock("Movement");
                break;

            case "MirrorSpecialStart":
                SetAnimState(PlayerAnim.AnimState.HeavyAttack);
                shield.SetActive(true);
                ms.IsSpecialAttack = true;
                //invincibility.StartAnimInvincible(4);
                SetReasonLock("Movement");
                break;
            #endregion

            #region Ninja
            case "NinjaLightStart":
                SetAnimState(PlayerAnim.AnimState.LightAttack);
                break;

            case "NinjaLightEnd":
                for (int i = -1; i <= 1; i++)
                {
                    SpawnKunai(i * 45).GetComponent<AttackDealer>().UpdateAttackDealer(pi.lightWeapon);
                }
                break;

            case "NinjaHeavyStart":
                SetAnimState(PlayerAnim.AnimState.HeavyAttack);
                ChangeSpeed(2);
                break;

            case "NinjaHeavyEnd":
                for (int i = 0; i < 10; i++)
                {
                    SpawnKunai(Random.Range(-45, 45)).GetComponent<AttackDealer>().UpdateAttackDealer(pi.heavyWeapon);
                }
                ResetSpeed();
                break;

            case "NinjaSpecialStart":
                SetAnimState(PlayerAnim.AnimState.SpecialAttack);
                break;

            case "NinjaSpecialEnd":
                bulletHellWaves = 300;
                break;
            #endregion

            case "MirrorHeavyEnd":
            case "MirrorSpecialEnd":
                SetReasonUnlock("Movement");
                break;

            case "HammerLightEnd":
            case "MirrorLightEnd":
                ResetSpeed();
                break;

            default:
                Debug.Log("Unknown method name: " + methodName);
                break;
        }
    }

    //Extra methods.
    #region Cutter
    [Header("Cutter Parameters")]
    [SerializeField] GameObject cutterPivot;
    private int nextWave = 0;

    private void CutterStart(PlayerAnim.AnimState state, float speed)
    {
        SetAnimState(state);
        ChangeSpeed(speed);
    }

    private void SpawnCutter()
    {
        GameObject projectile = poolController.GetObjectFromPool("CutterPool");
        projectile.transform.position = cutterPivot.transform.position;
        projectile.transform.eulerAngles = new(0, 0, pi.transform.eulerAngles.y < 90 ? 0 : 180);
        projectile.GetComponent<AttackDealer>().UpdateAttackDealer(pi.heavyWeapon);
    }

    private void CutterSpecial(int number)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject projectile = poolController.GetObjectFromPool("CutterPool");
            projectile.transform.position = cutterPivot.transform.position;

            Vector3 rot = projectile.transform.eulerAngles;
            rot.z = 360 / number * i;
            projectile.transform.eulerAngles = rot;

            projectile.transform.parent = cutterPivot.transform;
            projectile.GetComponent<AttackDealer>().UpdateAttackDealer(pi.specialWeapon);
        }
        nextWave = number * 2;
    }
    #endregion

    #region Ninja
    [Header("Ninja Parameters")]
    [SerializeField] private Transform firePoint;
    private float angle;
    private int bulletHellWaves = 0;
    private readonly Timer ninjaTimer = new(0.05f);
    private GameObject SpawnKunai(float angle)
    {
        GameObject projectile = poolController.GetObjectFromPool("KunaiPool");
        projectile.transform.position = firePoint.position;

        Vector3 rot = projectile.transform.eulerAngles;
        rot.z = angle;
        projectile.transform.eulerAngles = rot;

        return projectile;
    }
    #endregion

    #region Mirror
    [Header("Mirror Parameters")]
    [SerializeField] GameObject mirror;
    [SerializeField] GameObject shield;
    private MirrorShieldMovement ms;
    #endregion

    #region Jet
    [Header("Jet Parameters")]
    [SerializeField] private float backSpd = 5;
    [SerializeField] private float lightSpd = 7;
    [SerializeField] private float heavySpd = 11;
    [SerializeField] private float specialSpd = 20;

    private enum DashDirection
    {
        Light,
        Heavy,
        Special,
        backward,
    }

    private void JetDash(DashDirection direction)
    {
        rb.velocity = transform.right * direction switch
        {
            DashDirection.Light => lightSpd,
            DashDirection.Heavy => heavySpd,
            DashDirection.Special => specialSpd,
            DashDirection.backward => -backSpd,
            _ => 0
        };
    }

    private void JetStart(PlayerAnim.AnimState state, DashDirection direction)
    {
        SetAnimState(state);
        SetReasonLock("Movement");
        JetDash(direction);
        pi.isSliding = true;
        jump.fallGravity.value = 0f;
    }

    #endregion

    #region Miscellaneous
    public void SetAnimState(PlayerAnim.AnimState state) //Public as it is used in PlayerAnim
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
    #endregion
}
