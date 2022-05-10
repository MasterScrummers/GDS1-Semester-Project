using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HealthComponent))]
public abstract class Enemy : MonoBehaviour, IAttackReceiver
{
    [SerializeField] protected WeaponBase.Affinity type;
    [SerializeField] protected bool randomiseAffinity = false;
    private WeaponBase.Affinity weakness; //An enemy will now have a weakness
    private WeaponBase.Affinity resist; //An enemy will now have a resistance

    protected RoomData inRoom;
    protected HealthComponent health;

    // Start is called before the first frame update
    protected virtual void Start() {
        health = GetComponent<HealthComponent>();

        int affinityNum = typeof(WeaponBase.Affinity).GetEnumValues().Length;
        if (randomiseAffinity)
        {
            type = (WeaponBase.Affinity)Random.Range(0, affinityNum);
        }
        weakness = (WeaponBase.Affinity)(((int)type + 1) % affinityNum);
        resist = weakness + 1;

        SpriteOutliner outliner = GetComponentInChildren<SpriteOutliner>();
        outliner.SetColour(DoStatic.GetGameController<VariableController>().GetColor(type));
    }
    
    // Update is called once per frame
    protected virtual void Update()
    {
        if (health.health <= 0) {
            // Death();
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
        if (inRoom)
        {
            inRoom.UpdateEnemyCount();
        }

        gameObject.SetActive(false);
    }

    /// <summary>
    /// Can be overridden for taking damage
    /// </summary>
    /// <param name="damage">
    /// The amount of damage the enemy should take
    /// </param>
    public virtual void TakeDamage(int damage)
    {
        if (health.health <= 0)
        {
            Death();
        }
    }

    public void AssignToRoomData(RoomData roomData)
    {
        inRoom = roomData;
    }

    public void RecieveAttack(Transform attackPos, int strength, float knockbackStr, float invincibilityTime, WeaponBase.Affinity typing)
    {
        float extra = typing == weakness ? 1.25f : typing == resist ? 0.75f : 1f;
        health.TakeDamage((int)(strength * extra));
    }
}
