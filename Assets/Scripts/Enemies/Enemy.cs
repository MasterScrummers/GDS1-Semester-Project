using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HealthComponent))]
public abstract class Enemy : AttackDealer, IAttackReceiver
{
    [SerializeField] protected bool allowKnockback = true;
    [SerializeField] private Timer hurtTimer = new(0.2f);
    [SerializeField] private Timer stunnedTimer = new(0f);
    [SerializeField] protected bool isInvincibile = false;

    protected bool isStunned = false;
    protected Rigidbody2D rb;
    private SpriteRenderer sr;

    protected RoomData inRoom;
    protected HealthComponent health;

    protected virtual void Start()
    {
        health = GetComponent<HealthComponent>();

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Update()
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
    }

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

    public virtual void RecieveAttack(Transform attackerPos, WeaponBase weapon)
    {
        if (isInvincibile)
        {
            return;
        }

        health.OffsetHP(-weapon.strength * weapon.strengthMult);

        sr.color = Color.red;
        stunnedTimer.SetTimer(weapon.stunTime);

        if (allowKnockback)
        {
            rb.AddForce((transform.position - attackerPos.position).normalized * weapon.knockback, ForceMode2D.Impulse);
        }

        if (health.health <= 0)
        {
            Death();
        }
    }
}
