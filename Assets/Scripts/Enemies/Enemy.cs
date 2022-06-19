using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(Animator))]
[RequireComponent(typeof(HealthComponent))]
public abstract class Enemy : AttackDealer, IAttackReceiver
{
    [Header("Enemy Parameters")]
    [SerializeField] protected bool allowKnockback = true;
    [SerializeField] protected bool isHarmless = false;
    [SerializeField] protected bool isInvincible = false;
    [SerializeField] private Timer hurtTimer = new(0.2f);
    [SerializeField] private Timer despawnTimer = new(1f);
    private Timer stunnedTimer = new(0f);

    protected bool isStunned = false;
    protected Rigidbody2D rb;
    private SpriteRenderer sr;

    protected RoomData inRoom;
    protected HealthComponent health;

    protected virtual void Start()
    {
        weapon = new EnemyWeaponBase(isHarmless ? 0 : 1);
        health = GetComponent<HealthComponent>();

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();

        hurtTimer.Reset();
        despawnTimer.Reset();
    }

    /// <summary>
    /// NO OVERRIDING ALLOWED!
    /// </summary>
    protected override void Update()
    {
        base.Update();
        float delta = Time.deltaTime;

        if (health.health == 0)
        {
            despawnTimer.Update(delta);
            if (despawnTimer.tick == 0)
            {
                if (inRoom)
                {
                    inRoom.UpdateEnemyCount();
                    inRoom = null; //To prevent the count to go down more than once.
                }
                gameObject.SetActive(false);
            } else
            {
                DeathAction();
            }
            return;
        }

        hurtTimer.Update(delta);
        if (hurtTimer.tick == 0)
        {
            sr.color = Color.white;
            hurtTimer.Reset();
        }

        stunnedTimer.Update(delta);
        isStunned = stunnedTimer.tick > 0f;
        if (!isStunned)
        {
            DoAction();
        } else
        {
            DoStunnedAction();
        }
    }

    /// <summary>
    /// Treated as the Update method for children.
    /// </summary>
    protected virtual void DoAction() {}

    /// <summary>
    /// Treated as the Update method for children when stunned.
    /// </summary>
    protected virtual void DoStunnedAction() { }

    /// <summary>
    /// Treat as the Update method for children once dead.
    /// </summary>
    protected virtual void DeathAction() {}

    /// <summary>
    /// Meant to be overridden.
    /// </summary>
    protected virtual void Death() {}

    /// <summary>
    /// Should only be called once on startup.
    /// </summary>
    /// <param name="roomData">The roomdata needed to update upon death.</param>
    public void AssignToRoomData(RoomData roomData)
    {
        inRoom = roomData;
    }

    public void RecieveAttack(Transform attackerPos, WeaponBase weapon)
    {
        if (isInvincible)
        {
            return;
        }

        PlayerWeaponBase playerWeapon = (PlayerWeaponBase)weapon;
        health.OffsetHP(-playerWeapon.damage * playerWeapon.strengthMult);

        if (health.health <= 0)
        {
            Death();
            return;
        }

        sr.color = Color.red;
        if (allowKnockback)
        {
            stunnedTimer.SetTimer(playerWeapon.stunTime);
            rb.AddForce((transform.position - attackerPos.position).normalized * playerWeapon.knockback, ForceMode2D.Impulse);
        }

        HurtReaction();
    }

    /// <summary>
    /// Treated as RecieveAttack.
    /// </summary>
    protected virtual void HurtReaction() {}
}
