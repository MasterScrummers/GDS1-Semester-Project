using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : WeaponBase
{
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange;
    public LayerMask enemyLayers;

    // Start is called before the first frame update
    protected override void Start()
    {
        weaponName = "Sword";
        description = "A classic representation for main character";
        weaponType = Affinity.water;
        base.Start();
    }

    public override void LightAttack()
    {
            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in enemiesHit)
            {
                Debug.Log("Hit");
            }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(attackPoint.position, attackRange);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            LightAttack();
        }
    }

}
