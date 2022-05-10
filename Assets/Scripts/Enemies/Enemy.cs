using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(AttackDealer))]
public abstract class Enemy : MonoBehaviour, IAttackReceiver
{
    [SerializeField] protected int strength;
    [SerializeField] protected WeaponBase.Affinity type;
    [SerializeField] protected bool randomiseAffinity = false;
    private WeaponBase.Affinity weakness; //An enemy will now have a weakness
    private WeaponBase.Affinity resist; //An enemy will now have a resistance

    protected RoomData inRoom;
    protected HealthComponent health;

    // Start is called before the first frame update
    protected virtual void Start() {
        GetComponent<AttackDealer>().SetStrengthMult(strength);
        health = GetComponent<HealthComponent>();

        int affinityNum = typeof(WeaponBase.Affinity).GetEnumValues().Length;
        if (randomiseAffinity)
        {
            type = (WeaponBase.Affinity)Random.Range(0, affinityNum);
        }
        weakness = (WeaponBase.Affinity)((int)(type - 1) % affinityNum);
        resist = (WeaponBase.Affinity)(((int)type + 1) % affinityNum);

        SpriteOutliner outliner = GetComponentInChildren<SpriteOutliner>();
        outliner.SetColour(DoStatic.GetGameController<VariableController>().GetColor(type));
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

    public virtual void RecieveAttack(Transform attackPos, int strength, float knockbackStr, float invincibilityTime, WeaponBase.Affinity typing)
    {
        float extra = typing == weakness ? 1.25f : typing == resist ? 0.75f : 1f;
        int damage = (int)(strength * extra);
        health.TakeDamage(damage == 0 ? 1 : damage);
        if (health.health <= 0)
        {
            Death();
        }
        Debug.Log("You can do more stuff here.");
    }
}
