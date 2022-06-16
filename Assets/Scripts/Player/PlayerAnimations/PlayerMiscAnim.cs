#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable IDE0051
using UnityEngine;

public class PlayerMiscAnim : MonoBehaviour
{
    public PlayerAnim.AnimState animState { get; private set; } = PlayerAnim.AnimState.Idle;
    public PlayerAnim.JumpState jumpState { get; private set; } = PlayerAnim.JumpState.Waiting;

    private enum AnimEndTypes { None, CutterHeavy, CutterSpecial, MirrorHeavy, NinjaLight, NinjaHeavy, NinjaSpecial }

    private InputController ic; // Input Controller
    private AudioController ac; // Audio Controller
    private PlayerInput pi; //The update the animation according to player input.
    private Animator anim;
    private JumpComponent jump;
    private Rigidbody2D rb;
    private PoolController poolController;
    private AttackDealer attacker;
    private PlayerInvincibility invincibility;

    private void Start()
    {
        ic = DoStatic.GetGameController<InputController>();
        poolController = ic.GetComponent<PoolController>();
        ac = ic.GetComponent<AudioController>();

        pi = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
        jump = GetComponent<JumpComponent>();
        rb = GetComponent<Rigidbody2D>();
        invincibility = GetComponent<PlayerInvincibility>();
        attacker = GetComponentInChildren<AttackDealer>();

        hammerHeavySpins.Reset();
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
                    SpawnKunai((360 / num * i) + angle).GetComponent<AttackDealer>().SetAttack(pi.specialWeapon);
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
                animState = PlayerAnim.AnimState.Idle;
                break;

            case "WalkStart":
                animState = PlayerAnim.AnimState.Run;
                break;

            #region Jump
            case "JumpStart":
                animState = PlayerAnim.AnimState.Jump;
                jumpState = PlayerAnim.JumpState.StartJump;
                break;

            case "JumpPeakStart":
                jumpState = PlayerAnim.JumpState.Peak;
                break;

            case "JumpFallStart":
                animState = PlayerAnim.AnimState.Jump;
                jumpState = PlayerAnim.JumpState.Descending;
                break;

            case "JumpLandStart":
                jumpState = PlayerAnim.JumpState.Waiting;
                break;
            #endregion

            #region Sword
            case "SwordLightStart":
                AnimAttack(PlayerAnim.AnimState.LightAttack, pi.speed.originalValue, 1, new(15, 0), 0.2f, 0.25f);
                break;

            case "SwordHeavyStart":
                AnimAttack(PlayerAnim.AnimState.HeavyAttack, 3, 2, new(20, 0), 0.3f, 0.25f);
                break;

            case "SwordHeavyStop":
                ic.SetInputReason("Movement", false);
                break;

            case "SwordSpecialStart":
                AnimAttack(PlayerAnim.AnimState.SpecialAttack, 15, 0.5f, new(-15, -15), 0.1f, 0.5f, true);
                break;
            #endregion

            #region Hammer
            case "HammerLightStart":
                AnimAttack(PlayerAnim.AnimState.LightAttack, 3, 1, new(17, 0), 0.5f, 0.3f, true);
                break;

            case "HammerHeavyStart":
                AnimAttack(PlayerAnim.AnimState.HeavyAttack, pi.speed.originalValue, 2, new(17, 0), 0.3f, 0.3f);
                DashStart(jetLightSpd, false);
                if (hammerHeavySpins.value-- == 0)
                {
                    anim.SetTrigger("Finish");
                }
                break;

            case "HammerSpecialStart":
                AnimAttack(PlayerAnim.AnimState.SpecialAttack, 1, 2, new(10, 0), 0.1f, 0.2f);
                ac.PlaySound("HammerSpecial_0");
                break;

            case "HammerFlip":
                AnimAttack(PlayerAnim.AnimState.SpecialAttack, 1, 5, new(27, 35), 1f, 1f);
                ic.SetInputReason("Movement", false);
                break;

            case "HammerFlipExplode":
                ac.PlaySound("HammerSpecial_1");
                break;
            #endregion

            #region Cutter
            case "CutterLightStart":
                AnimAttack(PlayerAnim.AnimState.LightAttack, 7, 1, new(10, 0), 0.3f, 0.2f);
                break;

            case "CutterHeavyStart": //This uses projectile, fix this.
                AnimAttack(PlayerAnim.AnimState.HeavyAttack, 2, 2, new(10, 0), 0.3f, 0.2f);
                break;

            case "CutterSpecialStart":
                AnimAttack(PlayerAnim.AnimState.SpecialAttack, pi.speed.originalValue, 3, new(10, 0), 0.3f, 0.2f);
                break;
            #endregion

            #region Jet
            case "JetLightStart":
                AnimAttack(PlayerAnim.AnimState.LightAttack, 1, 1, new(20, 0), 0.3f, 0.2f);
                DashStart(jetLightSpd);
                break;

            case "JetHeavyStart":
                AnimAttack(PlayerAnim.AnimState.LightAttack, 1, 2, new(40, 0), 0.3f, 0.2f);
                DashStart(jetHeavySpd);
                break;

            case "JetSpecialStart":
                AnimAttack(PlayerAnim.AnimState.SpecialAttack, 1, 3, new(50, 0), 0.5f, 1);
                DashStart(jetSpecialSpd);
                break;

            case "JetBackStart":
                DashStart(-jetBackSpd);
                break;
            #endregion

            #region Mirror
            case "MirrorLightStart":
                AnimAttack(PlayerAnim.AnimState.LightAttack, 3, 1, new(20, 0), 0.5f, 1);
                break;

            case "MirrorLightActivate":
                AnimAttack(PlayerAnim.AnimState.LightAttack, 2, 1, new(20, 0), 0.5f, 1);
                break;

            case "MirrorHeavyStart":
                AnimAttack(PlayerAnim.AnimState.HeavyAttack, 1, 1, new(0, 0), 0.5f, 1);
                ic.SetInputReason("Movement", false);
                break;

            case "MirrorSpecialStart":
                AnimAttack(PlayerAnim.AnimState.SpecialAttack, 1, 1, new(0, 0), 0.1f, 1);
                ic.SetInputReason("Movement", false);
                break;
            #endregion

            #region Ninja
            case "NinjaLightStart":
                AnimAttack(PlayerAnim.AnimState.LightAttack, pi.speed.originalValue, 1, new(1f, 0), 0.1f, 0.1f);
                break;

            case "NinjaHeavyStart":
                AnimAttack(PlayerAnim.AnimState.HeavyAttack, 2, 1, new(1f, 0), 0.1f, 0.1f);
                break;

            case "NinjaSpecialStart":
                AnimAttack(PlayerAnim.AnimState.SpecialAttack, pi.speed.originalValue, 1, new(1f, 0), 0.1f, 0.1f);
                break;
            #endregion

            default:
                Debug.Log("Unknown method name: " + methodName);
                break;
        }
    }

    private void AnimAttack(PlayerAnim.AnimState state, float playerSpeed, float strengthMult, Vector2 knockback, float hitInterval, float stunTime, bool calcFromAttackerPos = false)
    {
        animState = state;
        attacker.SetAttack(strengthMult, knockback, hitInterval, stunTime, calcFromAttackerPos);
        pi.speed.value = playerSpeed;
    }

    private void AnimReset(AnimEndTypes animEnd)
    {
        switch (animEnd)
        {
            case AnimEndTypes.CutterHeavy:
                GameObject projectile = poolController.GetObjectFromPool("CutterPool");
                projectile.transform.position = cutterPivot.transform.position;
                projectile.transform.eulerAngles = new(0, 0, spritePivot.localScale.x > 0 ? 0 : 180);
                projectile.GetComponent<AttackDealer>().SetAttack(pi.heavyWeapon);
                break;

            case AnimEndTypes.CutterSpecial:
                CutterSpecial(4);
                break;

            case AnimEndTypes.NinjaLight:
                for (int i = -1; i <= 1; i++)
                {
                    SpawnKunai(i * 45).GetComponent<AttackDealer>().SetAttack(pi.lightWeapon);
                }
                break;

            case AnimEndTypes.NinjaHeavy:
                for (int i = 0; i < 10; i++)
                {
                    SpawnKunai(Random.Range(-45, 45)).GetComponent<AttackDealer>().SetAttack(pi.heavyWeapon);
                }
                break;

            case AnimEndTypes.NinjaSpecial:
                bulletHellWaves = 300;
                break;
        }

        hammerHeavySpins.Reset();
        anim.ResetTrigger("Finish");

        ic.SetInputReason("Movement", true);
        if (!invincibility.allowFlashing)
        {
            invincibility.SetPlayerInvincible(false);
        }
        pi.speed.Reset();
        pi.isSliding = false;
        jump.fallGravity.Reset();
    }

    //Extra methods.
    #region Hammer
    [Header("Hammer Parameters")]
    [SerializeField] private OriginalValue<int> hammerHeavySpins = new(3);
    #endregion

    #region Cutter
    [Header("Cutter Parameters")]
    [SerializeField] private GameObject cutterPivot;
    private int nextWave = 0;

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
            projectile.GetComponent<AttackDealer>().SetAttack(pi.specialWeapon);
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

    #region Jet
    [Header("Jet Parameters")]
    [SerializeField] private float jetBackSpd = 5;
    [SerializeField] private float jetLightSpd = 7;
    [SerializeField] private float jetHeavySpd = 11;
    [SerializeField] private float jetSpecialSpd = 20;
    [SerializeField] private Transform spritePivot;

    private void DashStart(float speed, bool affectGravity = true)
    {
        ic.SetInputReason("Movement", false);
        rb.velocity = transform.right * speed * spritePivot.localScale.x;
        pi.isSliding = true;
        if (affectGravity)
        {
            jump.fallGravity.value = 0f;
        }
        invincibility.SetPlayerInvincible(true, false);
    }
    #endregion
}
