using System.Collections.Generic;
using UnityEngine;

public class AttackDealer : MonoBehaviour
{
    [Header("Attack Dealer Parameters")]
    [SerializeField] protected int strength = 1; //The strength the hit, should be controlled by another script.
    protected float strengthMult = 1;

    [field: SerializeField] public Vector2 knockback { get; protected set; } = Vector2.one; //The knockback to give.
    [field: SerializeField] public float hitInterval { get; protected set; } = 0.3f;
    [field: SerializeField] public float stunTime { get; protected set; } = 0.5f;
    [field: SerializeField] public bool calcFromAttackerPos { get; protected set; } = false;
    private readonly Dictionary<Collider2D, KeyValuePair<IAttackReceiver, Timer>> victims = new();

    private void Update()
    {
        float delta = Time.deltaTime;
        if (victims.Count == 0)
        {
            return;
        }

        Collider2D[] keys = new Collider2D[victims.Keys.Count];
        victims.Keys.CopyTo(keys, 0);
        foreach (Collider2D key in keys)
        {
            if (!victims.TryGetValue(key, out var value))
            {
                return;
            }

            Timer time = value.Value;
            time.Update(delta);
            if (time.tick == 0)
            {
                time.Reset();
                value.Key.RecieveAttack(transform, Mathf.RoundToInt(strength * strengthMult), knockback, stunTime, calcFromAttackerPos);
            }
        }
    }

    public void SetAttack(WeaponBase weapon)
    {
        strength = weapon.baseStrength;
    }

    public void SetAttack(float mult, Vector2 knockback, float hitInterval, float stunTime, bool towardsAttacker = false)
    {
        strengthMult = mult;
        this.knockback = knockback;
        this.hitInterval = hitInterval;
        this.stunTime = stunTime;
        this.calcFromAttackerPos = towardsAttacker;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground") && collision.TryGetComponent<IAttackReceiver>(out var receiver))
        {
            victims.Add(collision, new(receiver, new(hitInterval)));
            receiver.RecieveAttack(transform, Mathf.RoundToInt(strength * strengthMult), knockback, stunTime, calcFromAttackerPos);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (victims.ContainsKey(other))
        {
            victims.Remove(other);
        }
    }
}
