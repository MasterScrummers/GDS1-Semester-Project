using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HealthComponent))]
public abstract class Enemy : AttackDealer, IAttackReceiver
{
    [SerializeField] protected bool randomiseAffinity = false;
    private WeaponBase.Affinity weakness; //An enemy will now have a weakness
    private WeaponBase.Affinity resist; //An enemy will now have a resistance
    [SerializeField] bool damageKnockback = false;
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

        int affinityNum = typeof(WeaponBase.Affinity).GetEnumValues().Length;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        player = DoStatic.GetPlayer();

        if (randomiseAffinity)
        {
            typing = (WeaponBase.Affinity)Random.Range(0, affinityNum);
        }
        weakness = (WeaponBase.Affinity)((int)(typing - 1) % affinityNum);
        resist = (WeaponBase.Affinity)(((int)typing + 1) % affinityNum);

        SpriteOutliner outliner = GetComponentInChildren<SpriteOutliner>();
        outliner.SetColour(DoStatic.GetGameController<VariableController>().GetColor(typing));
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

    public virtual void RecieveAttack(Transform attackerPos, int strength, float knockbackStr, float invincibilityTime, float stunTime, WeaponBase.Affinity typing)
    {
        if (this.invincibilityTime !<= 0f)
        {
            float extra = typing == weakness ? 1.25f : typing == resist ? 0.75f : 1f;
            int damage = (int)(strength * extra);
            health.TakeDamage(damage == 0 ? 1 : damage);
            
            sr.color = Color.red;
            hurt = true;
            this.stunTime = stunTime;
            this.invincibilityTime = invincibilityTime;

            // rb.AddForce(Vector3.Normalize(transform.position - player.transform.position) * 500f);
            rb.AddForce(new Vector2(attackerPos.position.x > transform.position.x ? -knockbackStr : knockbackStr, 2) * 2f, ForceMode2D.Impulse);

            Debug.Log("Taking damage in Enemy");
            if (health.health <= 0)
            {
                Death();
            }
            Debug.Log("You can do more stuff here.");
            }
    }
}
