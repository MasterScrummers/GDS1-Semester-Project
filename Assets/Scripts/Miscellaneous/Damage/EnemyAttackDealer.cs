using UnityEngine;

public class EnemyAttackDealer : AttackDealer
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player"))
        {
            collision.GetComponentInChildren<PlayerAnim>().TakeDamage(collision.transform.position.x < transform.position.x ? -1 : 1);
        }
    }
}
