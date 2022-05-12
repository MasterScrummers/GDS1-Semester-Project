using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HealthComponent))]
public abstract class Enemy : AttackDealer, IAttackReceiver
{
    [SerializeField] protected bool randomiseAffinity = false;
    protected bool hurt;
    public const float HurtTime = 0.2f;
    private float hurtColourTimer = HurtTime;
    protected bool isStunned = false;
    protected Rigidbody2D rb;
    private SpriteRenderer sr;
    protected GameObject player; // Player

    protected RoomData inRoom;
    protected HealthComponent health;

    // Start is called before the first frame update
    protected virtual void Start() {
        health = GetComponent<HealthComponent>();

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        player = DoStatic.GetPlayer();
    }
    
    // Update is called once per frame
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
                hurtColourTimer -= Time.deltaTime;
            }
        }

        isStunned = (stunTime -= Time.deltaTime) > 0f;
        invincibilityTime -= Time.deltaTime;
    }

    /// <summary>
    /// Meant to be overridden for movement script. This is already called in Update()
    /// </summary>
    protected virtual void Move() {}

    /// <summary>
    /// Meant to be overridden for attack script
    /// </summary>
    protected virtual void Attack() {}

    /// <summary>
    /// Calls this once when there is death.
    /// </summary>
    protected virtual void Death()
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

    public void RecieveAttack(Transform attackerPos, int strength, Vector2 knockback, float invincibilityTime, float stunTime)
    {
        if (this.invincibilityTime !<= 0f)
        {
            health.TakeDamage(strength);
            
            sr.color = Color.red;
            hurt = true;
            this.stunTime = stunTime;
            this.invincibilityTime = invincibilityTime;

            rb.AddForce(attackerPos.position.x > transform.position.x ? -knockback : knockback, ForceMode2D.Impulse);
            if (health.health <= 0)
            {
                Death();
            }
        }
    }
}
