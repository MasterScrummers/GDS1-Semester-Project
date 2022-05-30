using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HealthComponent))]
public abstract class Enemy : AttackDealer, IAttackReceiver
{
    [SerializeField] protected bool allowKnockback = true;
    protected bool hurt;
    public const float HurtTime = 0.2f;
    private float hurtColourTimer = HurtTime;
    protected bool isStunned = false;
    protected Rigidbody2D rb;
    private SpriteRenderer sr;

    protected RoomData inRoom;
    protected HealthComponent health;

    protected virtual void Start() {
        health = GetComponent<HealthComponent>();

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }
    
    protected virtual void Update()
    {
        if (hurt)
        {
            if (hurtColourTimer < 0f)
            {
                sr.color = Color.white;
                hurt = false;
                hurtColourTimer = HurtTime;
            } else {
                sr.color = Color.red;
                hurtColourTimer -= Time.deltaTime;
            }
        }

        isStunned = (stunTime -= Time.deltaTime) > 0f;
        invincibilityTime -= Time.deltaTime;
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

    public virtual void RecieveAttack(Transform attackerPos, int strength, Vector2 knockback, float invincibilityTime, float stunTime)
    {
        if (this.invincibilityTime > 0f)
        {
            return;
        }
        health.OffsetHP(-strength);

        hurt = true;
        this.stunTime = stunTime;
        this.invincibilityTime = invincibilityTime;

        if (allowKnockback)
        {
            rb.AddForce(attackerPos.position.x > transform.position.x ? -knockback : knockback, ForceMode2D.Impulse);
        }

        if (health.health <= 0)
        {
            Death();
        }
    }
}
