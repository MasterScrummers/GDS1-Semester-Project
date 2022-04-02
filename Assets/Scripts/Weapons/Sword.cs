using UnityEngine;

public class Sword : WeaponBase
{
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange;
    [SerializeField] Vector2 attackSize;
    public LayerMask enemyLayers;

    // Start is called before the first frame update
    protected override void Start()
    {
        weaponName = "Sword";
        description = "Basic Weapon";
        weaponType = Affinity.water;
        base.Start();
    }

    public override void LightAttack(Animator anim)
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in enemiesHit)
        {
            Debug.Log("Light Attack");
        }
    }

    public override void HeavyAttack(Animator anim)
    {
        attackSize = new Vector2(1.0f, 0.5f);
        Collider2D[] enemiesHit = Physics2D.OverlapBoxAll(attackPoint.position, attackSize, 0, enemyLayers);
        foreach (Collider2D enemy in enemiesHit)
        {
            Debug.Log("Heavy Attack");
        }
    }

    public override void SpecialAttack(Animator anim)
    {
        attackSize = new Vector2(2.0f, 2.0f);
        Collider2D[] enemiesHit = Physics2D.OverlapBoxAll(attackPoint.position, attackSize, enemyLayers);
        foreach (Collider2D enemy in enemiesHit)
        {
            Debug.Log("Special Attack");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(attackPoint.position, attackRange);
        Gizmos.DrawCube(attackPoint.position, attackSize);
    }
}
