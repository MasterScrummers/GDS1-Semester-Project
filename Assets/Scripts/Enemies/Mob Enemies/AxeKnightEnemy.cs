using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeKnightEnemy : Enemy
{

    // Start is called before the first frame update
    void Start()
    {
        health = 1;
        type = WeaponBase.Affinity.fire;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // To be implemented
    protected override void Move()
    {
        
    }

    // To be implemented
    protected override void Attack()
    {
    }

    // To be implemented
    protected override void Death()
    {
    }
}
