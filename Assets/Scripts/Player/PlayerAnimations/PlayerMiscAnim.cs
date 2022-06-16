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
                    SpawnProjectile("CutterPool", (360 / num * i) + angle, pi.specialWeapon);
                }
                angle += 20;
            }
        }

        CutterUpdate();
        NinjaUpdate();
    }

    private void LightAttack()
    {
        AnimAttack(PlayerAnim.AnimState.LightAttack, pi.lightWeapon);
    }

    private void HeavyAttack()
    {
        AnimAttack(PlayerAnim.AnimState.HeavyAttack, pi.heavyWeapon);
    }

    private void SpecialAttack()
    {
        AnimAttack(PlayerAnim.AnimState.SpecialAttack, pi.specialWeapon);
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

            #region Hammer
            case "HammerHeavyStart":
                LightAttack();
                DashStart(jetLightSpd, false);
                if (hammerHeavySpins.value-- == 0)
                {
                    anim.SetTrigger("Finish");
                }
                break;

            case "HammerFlip":
                ((Hammer)attacker.weapon).HammerFlip();
                break;

            case "HammerFlipExplode":
                ac.PlaySound("HammerSpecial_1");
                break;
            #endregion

            #region Jet
            case "JetLightStart":
                LightAttack();
                DashStart(jetLightSpd);
                break;

            case "JetHeavyStart":
                HeavyAttack();
                DashStart(jetHeavySpd);
                break;

            case "JetSpecialStart":
                SpecialAttack();
                DashStart(jetSpecialSpd);
                break;

            case "JetBackStart":
                DashStart(-jetBackSpd);
                break;
            #endregion

            default:
                Debug.Log("Unknown method name: " + methodName);
                break;
        }
    }

    private void AnimAttack(PlayerAnim.AnimState state, WeaponBase weapon = null)
    {
        animState = state;
        if (weapon == null)
        {
            return;
        }

        attacker.SetWeapon(weapon);
        string sfx = weapon.sfx;
        if (!sfx.Equals(""))
        {
            ac.PlaySound(sfx);
        }
    }

    private void AnimReset(AnimEndTypes animEnd)
    {
        switch (animEnd)
        {
            case AnimEndTypes.CutterHeavy:
                AnimAttack(PlayerAnim.AnimState.HeavyAttack);
                Debug.Log(spritePivot.localScale.x);
                SpawnProjectile("CutterPool", spritePivot.localScale.x > 0 ? 0 : 180, pi.heavyWeapon);
                break;

            case AnimEndTypes.CutterSpecial:
                AnimAttack(PlayerAnim.AnimState.SpecialAttack);
                CutterSpecial(4);
                break;

            case AnimEndTypes.NinjaLight:
                AnimAttack(PlayerAnim.AnimState.LightAttack);
                for (int i = -1; i <= 1; i++)
                {
                    SpawnProjectile("KunaiPool", i * 45, pi.lightWeapon);
                }
                break;

            case AnimEndTypes.NinjaHeavy:
                AnimAttack(PlayerAnim.AnimState.HeavyAttack);
                for (int i = 0; i < 10; i++)
                {
                    SpawnProjectile("KunaiPool", Random.Range(-45, 45), pi.heavyWeapon);
                }
                break;

            case AnimEndTypes.NinjaSpecial:
                AnimAttack(PlayerAnim.AnimState.SpecialAttack);
                bulletHellWaves = 300;
                break;
        }

        hammerHeavySpins.Reset();
        anim.ResetTrigger("Finish");

        if (!invincibility.allowFlashing)
        {
            invincibility.SetPlayerInvincible(false);
        }
        pi.allowMovement = true;
        pi.speed.Reset();
        pi.isSliding = false;
        jump.fallGravity.Reset();
    }

    //Extra methods.
    #region Hammer
    [Header("Hammer Parameters")]
    [SerializeField] private OriginalValue<int> hammerHeavySpins = new(3);
    #endregion

    private void SpawnProjectile(string pool, float angle, WeaponBase weapon)
    {
        GameObject projectile = poolController.GetObjectFromPool(pool);
        projectile.transform.position = firePoint.position;
        projectile.transform.eulerAngles = new(0, 0, angle);

        projectile.GetComponent<AttackDealer>().SetWeapon(weapon);
    }

    #region Cutter
    [Header("Cutter Parameters")]
    [SerializeField] private GameObject cutterPivot;
    private int nextWave = 0;

    private void CutterSpecial(int number)
    {
        for (int i = 0; i < number; i++)
        {
            SpawnProjectile("CutterPool", 360 / number * i, pi.specialWeapon);
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
        rb.velocity = transform.right * speed * spritePivot.localScale.x;
        pi.allowMovement = false;
        pi.isSliding = true;
        if (affectGravity)
        {
            jump.fallGravity.value = 0f;
        }
        invincibility.SetPlayerInvincible(true, false);
    }
    #endregion
}
