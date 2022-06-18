using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(Animator))]
[RequireComponent(typeof(HealthComponent))]
public abstract class Enemy : AttackDealer, IAttackReceiver
{
    [Header("Enemy Parameters")]
    [SerializeField] protected bool allowKnockback = true;
    [SerializeField] protected bool isInvincible = false;
    [SerializeField] private Timer hurtTimer = new(0.2f);
    private Timer stunnedTimer = new(0f);

    protected bool isStunned = false;
    protected Rigidbody2D rb;
    private SpriteRenderer sr;

    protected RoomData inRoom;
    protected HealthComponent health;

    protected virtual void Start()
    {
        weapon = new EnemyWeaponBase();
        health = GetComponent<HealthComponent>();

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// NO OVERRIDING ALLOWED!
    /// </summary>
    void Update()
    {
        float delta = Time.deltaTime;
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
        }
    }

    /// <summary>
    /// Treated as the Update method for children.
    /// </summary>
    protected virtual void DoAction() {}

    /// <summary>
    /// Meant to be COMPLETELY overridden.
    /// </summary>
    protected virtual void Death()
    {
        RemoveEnemy();
    }

    public void RemoveEnemy()
    {
        if (inRoom)
        {
            inRoom.UpdateEnemyCount();
            inRoom = null; //To prevent the count to go down more than once.
        }

        gameObject.SetActive(false);
    }

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
        health.OffsetHP(-playerWeapon.strength * playerWeapon.strengthMult);

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
