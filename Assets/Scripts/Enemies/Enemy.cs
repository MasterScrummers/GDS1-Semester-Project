using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(AttackDetector))]
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected WeaponBase.Affinity type;

    public RoomData inRoom;
    protected HealthComponent health;

    // Start is called before the first frame update
    protected virtual void Start() {
        inRoom = transform.parent.parent.GetComponent<RoomData>();
        health = GetComponent<HealthComponent>();
    }
    
    // Update is called once per frame
    protected virtual void Update()
    {
        if (health.health <= 0) {
            Death();
        }
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
    /// Called when health <= 0. Can override for a unique Death script, otherwise just destroys the GameObject.
    /// </summary>
    protected virtual void Death()
    {
        Destroy(gameObject);
        inRoom.CheckEnemyCount();
    }

    /// <summary>
    /// Can be overridden for taking damage
    /// </summary>
    /// <param name="damage">
    /// The amount of damage the enemy should take
    /// </param>
    protected virtual void TakeDamage(int damage)
    {
        health.TakeDamage(-damage);
        if (health.health <= 0)
        {
            Death();
        }
    }
}
