using UnityEngine;

public class AxeKnightEnemy : Enemy
{
    /// <summary>
    /// Since WeaponBase.Affinity type is serialised to be adjustable on
    /// the editor, we can simply set the type on the prefab without
    /// coding it onto the script. Thereby no need to override start().
    /// 
    /// Up to you, if it is not on the editor, we can override start()
    /// and set it there. What you have right now is fine.
    /// </summary>

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
        base.Death();
    }
}
