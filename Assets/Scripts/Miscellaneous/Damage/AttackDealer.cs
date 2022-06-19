#pragma warning disable IDE1006 // Naming Styles

using System.Collections.Generic;
using UnityEngine;

public class AttackDealer : MonoBehaviour
{
    public WeaponBase weapon { get; protected set; }

    private readonly Dictionary<Collider2D, KeyValuePair<IAttackReceiver, Timer>> victims = new();

    protected virtual void Update()
    {
        if (victims.Count == 0)
        {
            return;
        }

        Collider2D[] keys = new Collider2D[victims.Keys.Count];
        victims.Keys.CopyTo(keys, 0);
        float delta = Time.deltaTime;
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
                value.Key.RecieveAttack(transform, weapon);
            }
        }
    }

    public void SetWeapon(WeaponBase weapon)
    {
        this.weapon = weapon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IAttackReceiver>(out var receiver))
        {
            victims.Add(collision, new(receiver, new(weapon.hitInterval)));
            receiver.RecieveAttack(transform, weapon);
        }else if (collision.name.Equals("KirbyBody"))
        {
            Debug.Log("Ok");
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
